using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Decides whether a member may be attached as a partner to a new application.
/// Create enforces only the in-flight (pending) hard-block; the party's own
/// section submission enforces the pending block (excluding the current form)
/// plus the remarriage-exception and required divorce evidence (spec:
/// partner-eligibility 2026-09-08, Rev 2).
/// </summary>
public class PartnerEligibilityService : IPartnerEligibilityService
{
    private const string PendingMessage =
        "This member already has an active application and cannot be taken as a partner.";

    private const string GroomNotEligibleMessage =
        "This groom is already married. He can only be registered again if he declares a second/third/fourth Nikah, or states that he is divorced or widowed.";

    private const string GroomTalaqEvidenceMessage =
        "This groom has declared that he is divorced. Please provide evidence of Talaq (divorce certificate).";

    private const string BrideNotEligibleMessage =
        "This bride is already married. She can only be registered again if her marital status is Divorced or Widowed.";

    private const string BrideKhulaEvidenceMessage =
        "This bride has declared that she is divorced. Please provide evidence of Khula (divorce certificate).";

    private readonly RishtanataDbContext _context;

    public PartnerEligibilityService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<PartnerEligibilityResult> ValidateCreateAsync(
        string partnerMembershipNo,
        bool partnerIsGroom,
        CancellationToken cancellationToken = default)
    {
        var forms = await LoadFormsAsync(partnerMembershipNo, cancellationToken);

        return IsPending(forms, excludeFormId: null)
            ? Deny(PendingMessage)
            : Allow();
    }

    public async Task<PartnerEligibilityResult> ValidateSectionAsync(
        string membershipNo,
        bool partnerIsGroom,
        bool declaresSubsequentNikah,
        bool isWidower,
        bool isDivorced,
        string divorceEvidence,
        string brideMaritalStatus,
        string brideDivorceEvidence,
        Guid? excludeFormId,
        CancellationToken cancellationToken = default)
    {
        var no = membershipNo.Trim();
        var forms = await LoadFormsAsync(no, cancellationToken);

        if (IsPending(forms, excludeFormId))
            return Deny(PendingMessage);

        if (!IsMarried(forms))
            return Allow();

        if (partnerIsGroom)
        {
            if (declaresSubsequentNikah || isWidower)
                return Allow();

            if (isDivorced)
                return string.IsNullOrWhiteSpace(divorceEvidence)
                    ? Deny(GroomTalaqEvidenceMessage)
                    : Allow();

            return Deny(GroomNotEligibleMessage);
        }

        if (string.Equals(brideMaritalStatus, "Widowed", StringComparison.OrdinalIgnoreCase))
            return Allow();

        if (string.Equals(brideMaritalStatus, "Divorced", StringComparison.OrdinalIgnoreCase))
            return string.IsNullOrWhiteSpace(brideDivorceEvidence)
                ? Deny(BrideKhulaEvidenceMessage)
                : Allow();

        return Deny(BrideNotEligibleMessage);
    }

    private async Task<List<MarriageApplicationForm>> LoadFormsAsync(
        string membershipNo, CancellationToken cancellationToken)
    {
        var no = membershipNo.Trim();

        return await _context.MarriageApplicationForms
            .AsNoTracking()
            .Include(x => x.MarriageApplication)
                .ThenInclude(x => x.Certificate)
            .Where(x => x.BridegroomMembershipNo == no || x.BrideMembershipNo == no)
            .ToListAsync(cancellationToken);
    }

    private static bool IsPending(
        IEnumerable<MarriageApplicationForm> forms, Guid? excludeFormId) =>
        forms.Any(f =>
            !IsExcluded(f, excludeFormId) &&
            f.MarriageApplication != null &&
            f.MarriageApplication.Certificate == null &&
            f.MarriageApplication.Status != ApplicationStatus.ApplicationRejected &&
            f.MarriageApplication.Status != ApplicationStatus.ApplicationApproved &&
            f.FormStage != MarriageFormStage.Completed);

    private static bool IsMarried(IEnumerable<MarriageApplicationForm> forms) =>
        forms.Any(f =>
            f.MarriageApplication?.Certificate != null ||
            f.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved);

    private static bool IsExcluded(MarriageApplicationForm f, Guid? excludeFormId) =>
        excludeFormId.HasValue &&
        (f.Id == excludeFormId.Value || f.MarriageApplicationId == excludeFormId.Value);

    private static PartnerEligibilityResult Allow() =>
        new() { IsAllowed = true };

    private static PartnerEligibilityResult Deny(string message) =>
        new() { IsAllowed = false, Message = message };
}
