using Domain.Enums;
using Infrastructure.DTOs.SharedSection;

namespace Application.Interfaces;

/// <summary>
/// Anonymous shared-section flow: token validation, section submission with
/// the signature-block stage advance, panel status, minting, regeneration and
/// revocation. The guardian/witness block advances AwaitingWitnesses →
/// AwaitingImamVerification once all three sections are recorded.
/// </summary>
public interface ISharedSectionService
{
    /// <summary>Validates a raw token against the stored hash.</summary>
    Task<SectionTokenStatus> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fills the section addressed by the token (upsert of the section row +
    /// flat mirror) and advances the block when complete. Denied/invalid
    /// requests produce no side effects.
    /// </summary>
    Task<SectionSubmitResult> SubmitSectionAsync(string token, SectionFillData data, CancellationToken cancellationToken = default);

    /// <summary>Panel status per section (guardian, witness 1, witness 2), including
    /// the raw token when a link is currently active.</summary>
    Task<IReadOnlyList<SectionLinkStatus>> GetSignatureLinksStatusAsync(Guid applicationFormId, CancellationToken cancellationToken = default);

    /// <summary>Mints a new token (hash + raw stored) and returns the raw token; throws if an active token exists.</summary>
    Task<string> GenerateSectionTokenAsync(Guid applicationFormId, SectionType section, string createdByMembershipNo, CancellationToken cancellationToken = default);

    /// <summary>Overwrites the existing token (hash + raw) and returns the new raw token.</summary>
    Task<string> RegenerateSectionTokenAsync(Guid applicationFormId, SectionType section, string createdByMembershipNo, CancellationToken cancellationToken = default);

    /// <summary>Revokes an existing token (sets RevokedAt, clears hash + raw). Idempotent:
    /// revoking an already-revoked or missing token's section throws InvalidOperationException;
    /// revoking an already-revoked token is a no-op.</summary>
    Task RevokeSectionTokenAsync(Guid applicationFormId, SectionType section, CancellationToken cancellationToken = default);
}
