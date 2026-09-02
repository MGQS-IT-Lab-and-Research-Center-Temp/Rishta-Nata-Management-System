using Application.Authorization;
using Infrastructure.DTOs.BrideGroom;

namespace Application.Interfaces;

/// <summary>
/// Stage-gated submission of the bridegroom's section onto the marriage form.
/// Cleanup: split out of IBridegroomService so section submission is separate
/// from BridegroomFormSection record management.
/// </summary>
public interface IBridegroomSectionService
{
    Task<StageAuthorizationResult> SubmitBridegroomSectionAsync(
        Guid userId, Guid applicationFormId, BridegroomSectionDto dto,
        CancellationToken cancellationToken = default);
}