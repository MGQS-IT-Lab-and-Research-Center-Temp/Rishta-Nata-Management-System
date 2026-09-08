namespace Application.Interfaces;

public interface IPartnerEligibilityService
{
    Task<PartnerEligibilityResult> ValidateAsync(
        string partnerMembershipNo,
        bool partnerIsGroom,
        bool groomDeclaresSubsequentNikah,
        bool groomIsWidower,
        bool groomIsDivorced,
        string brideMaritalStatus,
        CancellationToken cancellationToken = default);
}

public sealed record PartnerEligibilityResult
{
    public bool IsAllowed { get; init; }
    public string Message { get; init; } = string.Empty;
}
