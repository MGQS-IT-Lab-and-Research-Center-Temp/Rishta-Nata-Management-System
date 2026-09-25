using Application.Authorization;
using Application.DivorceEvidence;
using Infrastructure.DTOs.BrideGroom;

namespace Application.Interfaces;

/// <summary>
/// Stage-gated submission of the bridegroom's section onto the marriage form.
/// Cleanup: split out of IBridegroomService so section submission is separate
/// from BridegroomFormSection record management.
/// </summary>
public interface IBridegroomSectionService
{
    /// <param name="divorceEvidence">The Talaq certificate posted with this
    /// submission, or null. Required (here or already on file) when the groom
    /// declares HasDivorcedFormerWife; ignored otherwise (Gap 8).</param>
    Task<StageAuthorizationResult> SubmitBridegroomSectionAsync(
        string membershipNo, Guid applicationFormId, BridegroomSectionDto dto,
        DivorceEvidenceUpload? divorceEvidence,
        CancellationToken cancellationToken = default);
}