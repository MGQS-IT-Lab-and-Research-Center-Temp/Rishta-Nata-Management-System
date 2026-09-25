using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Interfaces;

public interface IPartnerEligibilityService
{
    // Create: partner's remarriage status is not yet declared, so only the
    // in-flight (pending) hard-block is enforced here.
    Task<PartnerEligibilityResult> ValidateCreateAsync(
        string partnerMembershipNo,
        bool partnerIsGroom,
        CancellationToken cancellationToken = default);

    // Section submission (Continue): the submitting party declares their own
    // remarriage status. Pending-check excludes this application (excludeFormId)
    // so the member is not blocked by the very form they are continuing.
    // Divorce evidence is the uploaded certificate (Gap 8): the section service
    // passes whether one is on file or arriving with this submission.
    Task<PartnerEligibilityResult> ValidateSectionAsync(
        string membershipNo,
        bool partnerIsGroom,
        bool declaresSubsequentNikah,
        bool isWidower,
        bool isDivorced,
        bool hasDivorceEvidence,
        BrideMaritalStatus? brideMaritalStatus,
        bool brideHasDivorceEvidence,
        Guid? excludeFormId,
        CancellationToken cancellationToken = default);
}

public sealed record PartnerEligibilityResult
{
    public bool IsAllowed { get; init; }
    public string Message { get; init; } = string.Empty;
}
