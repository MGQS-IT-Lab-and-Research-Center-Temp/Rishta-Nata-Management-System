using Application.Authorization;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Application.Services;

/// <summary>
/// The single implementation of the stage-authorization policy
/// (docs/stage-authorization-policy.md): the only place that maps a member +
/// application + stage to allow/deny.
/// </summary>
public class StageAuthorizationService : IStageAuthorizationService
{
    private const int HierarchyLevelJamaatPresident = 2;
    private const int HierarchyLevelNationalRishtanataSecretary = 4;
    private const int HierarchyLevelAmir = 5;
    private readonly RishtanataDbContext _context;
    private readonly ILogger<StageAuthorizationService> _logger;
    public StageAuthorizationService(RishtanataDbContext context, ILogger<StageAuthorizationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<StageAuthorizationResult> CanUserActAsync(Guid userId, Guid applicationFormId, ApplicationStage targetStage,
    CancellationToken cancellationToken = default)
    {
        var form = await LoadFormAsync(applicationFormId, cancellationToken);
        if (form is null)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");
        }
        if (form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormCompleted,
                "The form reached final approval and is locked.");
        }
        var member = await ResolveMemberAsync(userId, cancellationToken);
        if (!member.IsKnown)
        {
            return Deny(userId, applicationFormId, targetStage,
                member.FailureReason!.Value, member.FailureMessage!);
        }
        var roleGate = MatchesRequiredRole(member.Member!, form, targetStage);
        if (!roleGate.IsAllowed)
        {
            return Deny(userId, applicationFormId, targetStage,
                roleGate.Reason!.Value, roleGate.Message);
        }
        if (form.ApplicationStage != targetStage)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.WrongStage,
                $"Role matches, but the form is currently at " +
                $"{form.ApplicationStage?.ToString() ?? "no stage"}, not {targetStage}.");
        }
        return Allow(userId, applicationFormId, targetStage);
    }

    public async Task<StageAuthorizationResult> CanUserActAsync(
        Guid userId,
        Guid applicationFormId,
        MarriageFormStage targetStage,
        CancellationToken cancellationToken = default)
    {
        var form = await LoadFormAsync(applicationFormId, cancellationToken);
        if (form is null)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");
        }
        if (form.FormStage == MarriageFormStage.Completed ||
            form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormCompleted,
                "The form reached final approval and is locked.");
        }
        var member = await ResolveMemberAsync(userId, cancellationToken);
        if (!member.IsKnown)
        {
            return Deny(userId, applicationFormId, targetStage,
                member.FailureReason!.Value, member.FailureMessage!);
        }
        var roleGate = await MatchesRequiredWorkflowRoleAsync(
            member.Member!, form, targetStage, cancellationToken);
        if (!roleGate.IsAllowed)
        {
            return Deny(userId, applicationFormId, targetStage,
                roleGate.Reason!.Value, roleGate.Message);
        }
        if (form.FormStage != targetStage)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.WrongStage,
                $"Role matches, but the form is currently at " +
                $"{form.FormStage}, not {targetStage}.");
        }
        return Allow(userId, applicationFormId, targetStage);
    }

    private async Task<MarriageApplicationForm?> LoadFormAsync(Guid applicationFormId, CancellationToken cancellationToken) =>
        await _context.MarriageApplicationForms
            .Include(f => f.MarriageApplication)
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId ||
                     f.MarriageApplicationId == applicationFormId,
                cancellationToken);

    private sealed record ResolvedMember(JamaatMember? Member, bool IsKnown)
    {
        public StageAuthorizationDenyReason? FailureReason { get; init; }
        public string? FailureMessage { get; init; }
    }

    private async Task<ResolvedMember> ResolveMemberAsync(
        Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return new ResolvedMember(null, false)
            {
                FailureReason = StageAuthorizationDenyReason.NoMembershipClaim,
                FailureMessage = "No usable membership identity was supplied."
            };
        }
        var member = await _context.JamaatMembers
            .Include(m => m.MemberRoles)
                .ThenInclude(mr => mr.Role)
            .FirstOrDefaultAsync(m => m.Id == userId, cancellationToken);
        if (member is null)
        {
            return new ResolvedMember(null, false)
            {
                FailureReason = StageAuthorizationDenyReason.UnknownMember,
                FailureMessage =
                    $"User id {userId} does not resolve to any known member record."
            };
        }
        return new ResolvedMember(member, true);
    }

    private static StageAuthorizationResult MatchesRequiredRole(JamaatMember member, MarriageApplicationForm form, ApplicationStage targetStage)
    {
        switch (targetStage)
        {
            case ApplicationStage.ApplicantsReview:
                var isBride = MembershipNumbersMatch(member.ChandaNo, form.BrideMembershipNo);
                var isGroom = MembershipNumbersMatch(member.ChandaNo, form.BridegroomMembershipNo);
                return isBride || isGroom
                    ? StageAuthorizationResult.Allow()
                    : StageAuthorizationResult.Deny(
                        StageAuthorizationDenyReason.WrongRole,
                        $"Member '{member.ChandaNo}' is neither the bride nor the bridegroom named on this application.");
            case ApplicationStage.JamaatPresidentReview:
                return RequireHierarchyLevel(
                    member, HierarchyLevelJamaatPresident, "Jamaat President");
            case ApplicationStage.NationalRishtanataSecretaryVerification:
                return RequireHierarchyLevel(
                    member, HierarchyLevelNationalRishtanataSecretary,
                    "National Rishtanata Secretary");
            case ApplicationStage.AmirApproval:
                return RequireHierarchyLevel(
                    member, HierarchyLevelAmir, "National Amir");
            default:
                return StageAuthorizationResult.Deny(
                    StageAuthorizationDenyReason.WrongRole,
                    $"Stage {targetStage} has no responsible role mapped.");
        }
    }

    private async Task<StageAuthorizationResult> MatchesRequiredWorkflowRoleAsync(
        JamaatMember member,
        MarriageApplicationForm form,
        MarriageFormStage targetStage,
        CancellationToken cancellationToken)
    {
        switch (targetStage)
        {
            case MarriageFormStage.AwaitingBride:
                return MembershipNumbersMatch(member.ChandaNo, form.BrideMembershipNo)
                    ? StageAuthorizationResult.Allow()
                    : StageAuthorizationResult.Deny(
                        StageAuthorizationDenyReason.WrongRole,
                        $"Member '{member.ChandaNo}' is not the bride named on this application.");
            case MarriageFormStage.AwaitingBridegroom:
                return MembershipNumbersMatch(member.ChandaNo, form.BridegroomMembershipNo)
                    ? StageAuthorizationResult.Allow()
                    : StageAuthorizationResult.Deny(
                        StageAuthorizationDenyReason.WrongRole,
                        $"Member '{member.ChandaNo}' is not the bridegroom named on this application.");
            case MarriageFormStage.AwaitingWitnesses:
                return await MatchesWitnessSlotAsync(member, form, cancellationToken);
            case MarriageFormStage.AwaitingImamVerification:
                var isImamOrMissionary = member.MemberRoles.Any(mr =>
                    mr.Role.Name.Contains("imam", StringComparison.OrdinalIgnoreCase) ||
                    mr.Role.Name.Contains("missionary", StringComparison.OrdinalIgnoreCase));
                return isImamOrMissionary
                    ? StageAuthorizationResult.Allow()
                    : StageAuthorizationResult.Deny(
                        StageAuthorizationDenyReason.WrongRole,
                        $"Member '{member.ChandaNo}' holds " +
                        $"{(member.MemberRoles.Any() ? $"roles '{string.Join(", ", member.MemberRoles.Select(mr => mr.Role.Name))}'" : "no roles")}; " +
                        "an Officiating Imam or Missionary is required for this stage.");
            case MarriageFormStage.AwaitingJamaatPresident:
                return RequireHierarchyLevel(
                    member, HierarchyLevelJamaatPresident, "Jamaat President");
            case MarriageFormStage.AwaitingRishtanataSecretary:
                return RequireHierarchyLevel(
                    member, HierarchyLevelNationalRishtanataSecretary,
                    "National Rishtanata Secretary");
            case MarriageFormStage.AwaitingAmirApproval:
                return RequireHierarchyLevel(
                    member, HierarchyLevelAmir, "National Amir");
            default:
                return StageAuthorizationResult.Deny(
                    StageAuthorizationDenyReason.WrongRole,
                    $"Stage {targetStage} has no responsible role mapped.");
        }
    }

    private async Task<StageAuthorizationResult> MatchesWitnessSlotAsync(
        JamaatMember member,
        MarriageApplicationForm form,
        CancellationToken cancellationToken)
    {
        foreach (var (name, tel, position) in new[]
                 {
                     (form.WitnessOneName, form.WitnessOneTel, 1),
                     (form.WitnessTwoName, form.WitnessTwoTel, 2)
                 })
        {
            if (!NamesAndPhoneMatch(member.FullName, member.PhoneNo, name, tel))
            {
                continue;
            }
            var matchingCount = await _context.JamaatMembers
                .AsNoTracking()
                .Where(m => m.PhoneNo != null &&
                            m.FirstName != null && m.Surname != null)
                .ToListAsync(cancellationToken);
            var ambiguousCount = matchingCount.Count(m =>
                NamesAndPhoneMatch(m.FullName, m.PhoneNo, name, tel));
            return ambiguousCount > 1
                ? StageAuthorizationResult.Deny(
                    StageAuthorizationDenyReason.AmbiguousIdentityMatch,
                    $"Witness {position} identity is ambiguous: more than one " +
                    "member record matches the recorded name and telephone.")
                : StageAuthorizationResult.Allow();
        }
        return StageAuthorizationResult.Deny(
            StageAuthorizationDenyReason.WrongRole,
            $"Member '{member.ChandaNo}' does not match either witness recorded on this application.");
    }

    private static bool NamesAndPhoneMatch(
        string? memberFullName, string? memberPhone,
        string? recordedName, string? recordedPhone) =>
        !string.IsNullOrWhiteSpace(recordedName) &&
        !string.IsNullOrWhiteSpace(recordedPhone) &&
        !string.IsNullOrWhiteSpace(memberFullName) &&
        !string.IsNullOrWhiteSpace(memberPhone) &&
        string.Equals(NormalizeName(memberFullName), NormalizeName(recordedName),
            StringComparison.OrdinalIgnoreCase) &&
        string.Equals(memberPhone.Trim(), recordedPhone.Trim(),
            StringComparison.OrdinalIgnoreCase);

    private static string NormalizeName(string value) =>
        string.Join(' ', value.Split(' ', StringSplitOptions.RemoveEmptyEntries));

    private static StageAuthorizationResult RequireHierarchyLevel(
        JamaatMember member,
        int requiredLevel,
        string officeName)
    {
        var hasLevel = member.MemberRoles.Any(mr => mr.Role.HierarchyLevel == requiredLevel);
        if (!hasLevel)
        {
            var actual = member.MemberRoles.Any()
                ? string.Join(", ", member.MemberRoles.Select(mr => $"{mr.Role.Name} (level {mr.Role.HierarchyLevel})"))
                : "no roles";
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongRole,
                $"Member '{member.ChandaNo}' holds {actual}; " +
                $"{officeName} is required for this stage.");
        }
        return StageAuthorizationResult.Allow();
    }

    private static bool MembershipNumbersMatch(string? claimed, string? recorded) =>
        !string.IsNullOrWhiteSpace(claimed) &&
        !string.IsNullOrWhiteSpace(recorded) &&
        string.Equals(claimed.Trim(), recorded.Trim(), StringComparison.OrdinalIgnoreCase);

    private StageAuthorizationResult Allow(
        Guid userId,
        Guid applicationFormId,
        object targetStage)
    {
        _logger.LogDebug(
            "Stage authorization allowed: UserId={UserId}, ApplicationFormId={ApplicationFormId}, TargetStage={TargetStage}",
            userId, applicationFormId, targetStage);
        return StageAuthorizationResult.Allow();
    }

    private StageAuthorizationResult Deny(
        Guid userId,
        Guid applicationFormId,
        object targetStage,
        StageAuthorizationDenyReason reason,
        string message)
    {
        _logger.LogWarning(
            "Stage authorization denied: UserId={UserId}, ApplicationFormId={ApplicationFormId}, TargetStage={TargetStage}, Reason={Reason}, Detail={Detail}",
            userId, applicationFormId, targetStage, reason, message);
        return StageAuthorizationResult.Deny(reason, message);
    }
}