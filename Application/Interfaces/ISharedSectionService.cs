using Domain.Enums;
using Infrastructure.DTOs.SharedSection;

namespace Application.Interfaces;

/// <summary>
/// Anonymous shared-section flow: token validation, section submission with
/// the signature-block stage advance, panel status, minting and regeneration.
/// The guardian/witness block advances AwaitingWitnesses →
/// AwaitingImamVerification once all three sections are recorded.
/// </summary>
public interface ISharedSectionService
{
    /// <summary>Validates a raw token; never returns the raw token back.</summary>
    Task<SectionTokenStatus> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fills the section addressed by the token (upsert of the section row +
    /// flat mirror) and advances the block when complete. Denied/invalid
    /// requests produce no side effects.
    /// </summary>
    Task<SectionSubmitResult> SubmitSectionAsync(string token, SectionFillData data, CancellationToken cancellationToken = default);

    /// <summary>Panel status per section (guardian, witness 1, witness 2).</summary>
    Task<IReadOnlyList<SectionLinkStatus>> GetSignatureLinksStatusAsync(Guid applicationFormId, CancellationToken cancellationToken = default);

    /// <summary>Returns the RAW token exactly once; throws if an active token exists.</summary>
    Task<string> GenerateSectionTokenAsync(Guid applicationFormId, SectionType section, string createdByMembershipNo, CancellationToken cancellationToken = default);

    /// <summary>Revokes (overwrites) the existing token and returns the new RAW token.</summary>
    Task<string> RegenerateSectionTokenAsync(Guid applicationFormId, SectionType section, string createdByMembershipNo, CancellationToken cancellationToken = default);
}