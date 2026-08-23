using Application.Authorization;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

/// Each denial carries the most specific reason:
//   1. Form lookup            → FormNotFound
//   2. Final approval check   → FormCompleted
//   3. Membership resolution  → NoMembershipClaim / UnknownMember
//   4. Role gate              → WrongRole
//   5. Stage gate             → WrongStage
// Denied requests produce no side effects — this service never writes.
public class StageAuthorizationService : IStageAuthorizationService
{
    // Canonical mapping from ApplicationStage to the office-holder's role,
    // expressed via Role.HierarchyLevel (Domain/Entities/Role.cs):
    // 1 = Jama'at Member, 2 = Jama'at President, 3 = Circuit President,
    // 4 = National Rishtanata Secretary, 5 = Amir.
    private const int HierarchyLevelJamaatPresident = 2;
    private const int HierarchyLevelNationalRishtanataSecretary = 4;
    private const int HierarchyLevelAmir = 5;

    private readonly RishtanataDbContext _context;
    private readonly ILogger<StageAuthorizationService> _logger;

    public StageAuthorizationService(
        RishtanataDbContext context,
        ILogger<StageAuthorizationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<StageAuthorizationResult> CanUserActAsync(Guid userId, Guid applicationFormId, ApplicationStage targetStage,
    CancellationToken cancellationToken = default)
    {
        var form = await _context.MarriageApplicationForms
            .Include(f => f.MarriageApplication)
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId ||
                     f.MarriageApplicationId == applicationFormId,
                cancellationToken);

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

        if (userId == Guid.Empty)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.NoMembershipClaim,
                "No usable membership identity was supplied.");
        }

        var member = await _context.JamaatMembers
            .Include(m => m.Role)
            .FirstOrDefaultAsync(m => m.Id == userId, cancellationToken);

        if (member is null)
        {
            return Deny(userId, applicationFormId, targetStage,
                StageAuthorizationDenyReason.UnknownMember,
                $"User id {userId} does not resolve to any known member record.");
        }

        var roleGate = MatchesRequiredRole(member, form, targetStage);
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
                $"{form.FormStage.ToString() ?? "no stage"}, not {targetStage}.");
        }

        _logger.LogDebug(
            "Stage authorization allowed: UserId={UserId}, ApplicationFormId={ApplicationFormId}, TargetStage={TargetStage}",
            userId, applicationFormId, targetStage);

        return StageAuthorizationResult.Allow();
    }

    private static StageAuthorizationResult MatchesRequiredRole(
        JamaatMember member,
        MarriageApplicationForm form,
        ApplicationStage targetStage)
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

    private static StageAuthorizationResult RequireHierarchyLevel(
        JamaatMember member,
        int requiredLevel,
        string officeName)
    {
        if (member.Role is null || member.Role.HierarchyLevel != requiredLevel)
        {
            var actual = member.Role is null
                ? "no role"
                : $"{member.Role.Name} (hierarchy level {member.Role.HierarchyLevel})";

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

    private StageAuthorizationResult Deny(
        Guid userId,
        Guid applicationFormId,
        ApplicationStage targetStage,
        StageAuthorizationDenyReason reason,
        string message)
    {
        _logger.LogWarning(
            "Stage authorization denied: UserId={UserId}, ApplicationFormId={ApplicationFormId}, TargetStage={TargetStage}, Reason={Reason}, Detail={Detail}",
            userId, applicationFormId, targetStage, reason, message);

        return StageAuthorizationResult.Deny(reason, message);
    }
}