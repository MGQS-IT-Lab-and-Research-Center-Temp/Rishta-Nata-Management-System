using Application.Authorization;
using Infrastructure.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Stage-gated submission of the bride's section onto the marriage form.
/// Cleanup: split out of IBrideGuardianService so section submission is
/// separate from BrideGuardian record management.
/// </summary>
public interface IBrideSectionService
{
    Task<StageAuthorizationResult> SubmitBrideSectionAsync(
        Guid userId, Guid applicationFormId, BrideSectionDto dto,
        CancellationToken cancellationToken = default);
}