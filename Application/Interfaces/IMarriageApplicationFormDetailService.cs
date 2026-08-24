using Infrastructure.DTOs.MarriageApplicationFormDetail;

namespace Application.Interfaces;

/// <summary>
/// Read-side service that assembles the full display state of a marriage
/// application form in one round trip (Epic C3). CanCurrentUserEdit is
/// computed through IStageAuthorizationService so the UI and the API never
/// disagree about who can act (policy §7.3).
/// </summary>
public interface IMarriageApplicationFormDetailService
{
    /// <summary>
    /// Loads the form (with its owning application and rejection history) and
    /// maps it to the detail DTO, computing CanCurrentUserEdit for the
    /// currently authenticated user.
    /// </summary>
    /// <param name="applicationFormId">
    /// Id of the marriage application form or of its owning FormApplication.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The detail DTO, or null when no such form exists.</returns>
    Task<MarriageApplicationFormDetailDto?> GetDetailAsync(
        Guid applicationFormId,
        CancellationToken cancellationToken = default);
}