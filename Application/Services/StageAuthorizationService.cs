using Application.Authorization;
using Application.Interfaces;
using Domain.Constants;
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
    private readonly RishtanataDbContext _context;
    private readonly ILogger<StageAuthorizationService> _logger;
    public StageAuthorizationService(RishtanataDbContext context, ILogger<StageAuthorizationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<StageAuthorizationResult> CanUserActAsync(string membershipNo, Guid applicationFormId, ApplicationStage targetStage,
    CancellationToken cancellationToken = default)
    {
        var form = await LoadFormAsync(applicationFormId, cancellationToken);
        if (form is null)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");
        }
        if (form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormCompleted,
                "The form reached final approval and is locked.");
        }
        var member = await ResolveMemberAsync(membershipNo, cancellationToken);
        if (!member.IsKnown)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                member.FailureReason!.Value, member.FailureMessage!);
        }
        var roleGate = MatchesRequiredRole(member.Member!, form, targetStage);
        if (!roleGate.IsAllowed)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                roleGate.Reason!.Value, roleGate.Message);
        }
        if (form.ApplicationStage != targetStage)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                StageAuthorizationDenyReason.WrongStage,
                $"Role matches, but the form is currently at " +
                $"{form.ApplicationStage?.ToString() ?? "no stage"}, not {targetStage}.");
        }
        return Allow(membershipNo, applicationFormId, targetStage);
    }

    public async Task<StageAuthorizationResult> CanUserActAsync(
        string membershipNo,
        Guid applicationFormId,
        MarriageFormStage targetStage,
        CancellationToken cancellationToken = default)
    {
        var form = await LoadFormAsync(applicationFormId, cancellationToken);
        if (form is null)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");
        }
        if (form.FormStage == MarriageFormStage.Completed ||
            form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                StageAuthorizationDenyReason.FormCompleted,
                "The form reached final approval and is locked.");
        }
        var member = await ResolveMemberAsync(membershipNo, cancellationToken);
        if (!member.IsKnown)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                member.FailureReason!.Value, member.FailureMessage!);
        }
        var roleGate = await MatchesRequiredWorkflowRoleAsync(
            member.Member!, form, targetStage, cancellationToken);
        if (!roleGate.IsAllowed)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                roleGate.Reason!.Value, roleGate.Message);
        }
        if (form.FormStage != targetStage)
        {
            return Deny(membershipNo, applicationFormId, targetStage,
                StageAuthorizationDenyReason.WrongStage,
                $"Role matches, but the form is currently at " +
                $"{form.FormStage}, not {targetStage}.");
        }
        return Allow(membershipNo, applicationFormId, targetStage);
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
        string membershipNo, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(membershipNo))
        {
            return new ResolvedMember(null, false)
            {
                FailureReason = StageAuthorizationDenyReason.NoMembershipClaim,
                FailureMessage = "No usable membership identity was supplied."
            };
        }
        var member = await _context.JamaatMembers
            .FirstOrDefaultAsync(m => m.ChandaNo == membershipNo, cancellationToken);
        if (member is null)
        {
            return new ResolvedMember(null, false)
            {
                FailureReason = StageAuthorizationDenyReason.UnknownMember,
                FailureMessage =
                    $"Membership number {membershipNo} does not resolve to any known member record."
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
                return RequireRole(member, "Jamaat President", RoleNames.JamaatPresident);
            case ApplicationStage.NationalRishtanataSecretaryVerification:
                return RequireRole(member, "National Rishtanata Secretary", RoleNames.RishtanataSecretary);
            case ApplicationStage.AmirApproval:
                return RequireRole(member, "National Amir", RoleNames.Amir);
            case ApplicationStage.ImamSignoff:
                return RequireDesignatedImam(member, form);
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
            case MarriageFormStage.AwaitingBrideJamaatPresident:
                return await RequireJamaatPresidentForAsync(
                    member, form, form.BrideMembershipNo, cancellationToken);
            case MarriageFormStage.AwaitingGroomJamaatPresident:
                return await RequireJamaatPresidentForAsync(
                    member, form, form.BridegroomMembershipNo, cancellationToken);
            case MarriageFormStage.AwaitingRishtanataSecretary:
                return RequireRole(member, "National Rishtanata Secretary", RoleNames.RishtanataSecretary);
            case MarriageFormStage.AwaitingAmirApproval:
                return RequireRole(member, "National Amir", RoleNames.Amir);
            case MarriageFormStage.AwaitingImamSignoff:
                return RequireDesignatedImam(member, form);
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
        var memberFullName = BuildFullName(member.FirstName, member.Surname);

        foreach (var (membershipNo, name, tel, position) in new[]
                 {
                     (form.WitnessOneMembershipNo, form.WitnessOneName, form.WitnessOneTel, 1),
                     (form.WitnessTwoMembershipNo, form.WitnessTwoName, form.WitnessTwoTel, 2)
                 })
        {
            // Preferred (Kind A): witness ChandaNo was captured on the section—
            // match exactly like Kind A; the name/telephone fallback never applies.
            if (!string.IsNullOrWhiteSpace(membershipNo))
            {
                if (MembershipNumbersMatch(member.ChandaNo, membershipNo))
                {
                    return StageAuthorizationResult.Allow();
                }

                continue;
            }

            // Fallback (Kind B): no ChandaNo recorded — match on name AND telephone.
            if (!NamesAndPhoneMatch(memberFullName, member.PhoneNo, name, tel))
            {
                continue;
            }

            var ambiguousCount = await CountAmbiguousWitnessMatchesAsync(
                name, tel, cancellationToken);

            if (ambiguousCount > 1)
            {
                return StageAuthorizationResult.Deny(
                    StageAuthorizationDenyReason.AmbiguousIdentityMatch,
                    $"Witness {position} identity is ambiguous: more than one " +
                    "member record matches the recorded name and telephone.");
            }

            return StageAuthorizationResult.Allow();
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

    private static StageAuthorizationResult RequireRole(
        JamaatMember member,
        string officeName,
        params string[] roleNames)
    {
        if (HasRole(member, roleNames))
        {
            return StageAuthorizationResult.Allow();
        }

        var actual = string.IsNullOrWhiteSpace(member.Roles)
            ? "no roles"
            : $"roles '{member.Roles}'";

        return StageAuthorizationResult.Deny(
            StageAuthorizationDenyReason.WrongRole,
            $"Member '{member.ChandaNo}' holds {actual}; " +
            $"{officeName} is required for this stage.");
    }

    private static StageAuthorizationResult RequireImamOrMissionary(JamaatMember member)
    {
        if (HasRoleContaining(member, "imam", "missionary"))
        {
            return StageAuthorizationResult.Allow();
        }

        var actual = string.IsNullOrWhiteSpace(member.Roles)
            ? "no roles"
            : $"roles '{member.Roles}'";

        return StageAuthorizationResult.Deny(
            StageAuthorizationDenyReason.WrongRole,
            $"Member '{member.ChandaNo}' holds {actual}; " +
            "an Officiating Imam or Missionary is required for this stage.");
    }

    /// <summary>
    /// President gate: the principal must hold the Jamaat President role AND be
    /// the president of the Jama'at the given partner belongs to. The partner's
    /// Jama'at is resolved from their member record by ChandaNo.
    /// </summary>
    private async Task<StageAuthorizationResult> RequireJamaatPresidentForAsync(
        JamaatMember member,
        MarriageApplicationForm form,
        string partnerMembershipNo,
        CancellationToken cancellationToken)
    {
        var roleGate = RequireRole(member, "Jamaat President", RoleNames.JamaatPresident);
        if (!roleGate.IsAllowed)
        {
            return roleGate;
        }

        var partnerJamaat = await _context.JamaatMembers
            .Where(p => p.ChandaNo == partnerMembershipNo)
            .Select(p => p.JamaatName)
            .FirstOrDefaultAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(partnerJamaat)
            && string.Equals(member.JamaatName.Trim(), partnerJamaat.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return StageAuthorizationResult.Allow();
        }

        var actual = string.IsNullOrWhiteSpace(member.JamaatName)
            ? "no Jama'at assigned"
            : $"the president of '{member.JamaatName}'";

        return StageAuthorizationResult.Deny(
            StageAuthorizationDenyReason.WrongRole,
            $"Member '{member.ChandaNo}' is {actual}; the partner on this application belongs to '{partnerJamaat}'.");
    }

    /// <summary>
    /// Imam gate: the principal must hold an imam/missionary role AND be the very
    /// imam the National Rishtanata office designated for this application.
    /// </summary>
    private static StageAuthorizationResult RequireDesignatedImam(
        JamaatMember member,
        MarriageApplicationForm form)
    {
        if (!HasRoleContaining(member, "imam", "missionary"))
        {
            var actual = string.IsNullOrWhiteSpace(member.Roles)
                ? "no roles"
                : $"roles '{member.Roles}'";

            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongRole,
                $"Member '{member.ChandaNo}' holds {actual}; " +
                "an Officiating Imam or Missionary is required for this stage.");
        }

        if (MembershipNumbersMatch(member.ChandaNo, form.OfficiatingImamMembershipNo))
        {
            return StageAuthorizationResult.Allow();
        }

        return StageAuthorizationResult.Deny(
            StageAuthorizationDenyReason.WrongRole,
            $"Member '{member.ChandaNo}' is not the Imam designated to officiate this application.");
    }

    private static bool HasRole(JamaatMember member, params string[] roleNames)
    {
        if (string.IsNullOrWhiteSpace(member.Roles))
        {
            return false;
        }

        var roles = member.Roles
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return roles.Any(role =>
            roleNames.Any(expected =>
                string.Equals(role, expected, StringComparison.OrdinalIgnoreCase)));
    }

    private static bool HasRoleContaining(JamaatMember member, params string[] fragments)
    {
        if (string.IsNullOrWhiteSpace(member.Roles))
        {
            return false;
        }

        var roles = member.Roles
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return roles.Any(role =>
            fragments.Any(fragment =>
                role.Contains(fragment, StringComparison.OrdinalIgnoreCase)));
    }

    private async Task<int> CountAmbiguousWitnessMatchesAsync(
        string recordedName,
        string recordedTel,
        CancellationToken cancellationToken)
    {
        var tel = recordedTel.Trim();
        var normalizedName = NormalizeName(recordedName);

        var candidates = await _context.JamaatMembers
            .AsNoTracking()
            .Where(m => m.PhoneNo != null && m.PhoneNo == tel)
            .Select(m => new { m.FirstName, m.Surname })
            .ToListAsync(cancellationToken);

        return candidates.Count(m =>
            string.Equals(
                NormalizeName(BuildFullName(m.FirstName, m.Surname)),
                normalizedName,
                StringComparison.OrdinalIgnoreCase));
    }

    private static string BuildFullName(string? firstName, string? surname) =>
        $"{firstName} {surname}";

    private static bool MembershipNumbersMatch(string? claimed, string? recorded) =>
        !string.IsNullOrWhiteSpace(claimed) &&
        !string.IsNullOrWhiteSpace(recorded) &&
        string.Equals(claimed.Trim(), recorded.Trim(), StringComparison.OrdinalIgnoreCase);

    private StageAuthorizationResult Allow(
        string membershipNo,
        Guid applicationFormId,
        object targetStage)
    {
        _logger.LogDebug(
            "Stage authorization allowed: MembershipNo={MembershipNo}, ApplicationFormId={ApplicationFormId}, TargetStage={TargetStage}",
            membershipNo, applicationFormId, targetStage);
        return StageAuthorizationResult.Allow();
    }

    private StageAuthorizationResult Deny(
        string membershipNo,
        Guid applicationFormId,
        object targetStage,
        StageAuthorizationDenyReason reason,
        string message)
    {
        _logger.LogWarning(
            "Stage authorization denied: MembershipNo={MembershipNo}, ApplicationFormId={ApplicationFormId}, TargetStage={TargetStage}, Reason={Reason}, Detail={Detail}",
            membershipNo, applicationFormId, targetStage, reason, message);
        return StageAuthorizationResult.Deny(reason, message);
    }
}