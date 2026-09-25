using Application.Authorization;
using Application.DivorceEvidence;
using Infrastructure.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Stage-gated submission of the bride's section onto the marriage form.
/// Cleanup: split out of IBrideGuardianService so section submission is
/// separate from BrideGuardian record management.
/// </summary>
public interface IBrideSectionService
{
    /// <param name="divorceEvidence">The Khula certificate posted with this
    /// submission, or null. Required (here or already on file) when the bride
    /// declares DivorcedIddatComplete; ignored otherwise (Gap 8).</param>
    Task<StageAuthorizationResult> SubmitBrideSectionAsync(
        string membershipNo, Guid applicationFormId, BrideSectionDto dto,
        DivorceEvidenceUpload? divorceEvidence,
        CancellationToken cancellationToken = default);
}