using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Decides whether a member may be attached as a partner to a new application,
/// enforcing the double-match guard: a party to another in-flight form is always
/// blocked, and a member with a recorded marriage is only reusable through the
/// legitimate remarriage declarations (spec: partner-eligibility 2026-09-08).
/// </summary>
public class PartnerEligibilityService : IPartnerEligibilityService
{
    private readonly RishtanataDbContext _context;

    public PartnerEligibilityService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<PartnerEligibilityResult> ValidateAsync(
        string partnerMembershipNo,
        bool partnerIsGroom,
        bool groomDeclaresSubsequentNikah,
        bool groomIsWidower,
        bool groomIsDivorced,
        string brideMaritalStatus,
        CancellationToken cancellationToken = default)
    {
        var no = partnerMembershipNo.Trim();

        var forms = await _context.MarriageApplicationForms
            .AsNoTracking()
            .Include(x => x.MarriageApplication)
                .ThenInclude(x => x.Certificate)
            .Where(x => x.BridegroomMembershipNo == no || x.BrideMembershipNo == no)
            .ToListAsync(cancellationToken);

        var pending = forms.Any(f =>
            f.MarriageApplication != null &&
            f.MarriageApplication.Certificate == null &&
            f.MarriageApplication.Status != ApplicationStatus.ApplicationRejected &&
            f.MarriageApplication.Status != ApplicationStatus.ApplicationApproved &&
            f.FormStage != MarriageFormStage.Completed);

        if (pending)
        {
            return new PartnerEligibilityResult
            {
                IsAllowed = false,
                Message = "This member already has an active application and cannot be taken as a partner."
            };
        }

        var married = forms.Any(f =>
            f.MarriageApplication?.Certificate != null ||
            f.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved);

        if (!married)
        {
            return new PartnerEligibilityResult { IsAllowed = true };
        }

        if (partnerIsGroom)
        {
            var eligible = groomDeclaresSubsequentNikah || groomIsWidower || groomIsDivorced;

            return eligible
                ? new PartnerEligibilityResult { IsAllowed = true }
                : new PartnerEligibilityResult
                {
                    IsAllowed = false,
                    Message = "This groom is already married. He can only be registered again if he declares a second/third/fourth Nikah, or states that he is divorced or widowed."
                };
        }

        var brideEligible =
            brideMaritalStatus.Equals("Divorced", StringComparison.OrdinalIgnoreCase) ||
            brideMaritalStatus.Equals("Widowed", StringComparison.OrdinalIgnoreCase);

        return brideEligible
            ? new PartnerEligibilityResult { IsAllowed = true }
            : new PartnerEligibilityResult
            {
                IsAllowed = false,
                Message = "This bride is already married. She can only be registered again if her marital status is Divorced or Widowed."
            };
    }
}
