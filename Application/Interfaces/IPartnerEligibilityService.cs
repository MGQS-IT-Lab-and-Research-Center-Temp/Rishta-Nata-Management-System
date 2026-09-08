using System;
using System.Threading;
using System.Threading.Tasks;

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
    Task<PartnerEligibilityResult> ValidateSectionAsync(
        string membershipNo,
        bool partnerIsGroom,
        bool declaresSubsequentNikah,
        bool isWidower,
        bool isDivorced,
        string divorceEvidence,
        string brideMaritalStatus,
        string brideDivorceEvidence,
        Guid? excludeFormId,
        CancellationToken cancellationToken = default);
}

public sealed record PartnerEligibilityResult
{
    public bool IsAllowed { get; init; }
    public string Message { get; init; } = string.Empty;
}
