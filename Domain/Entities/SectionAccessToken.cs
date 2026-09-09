using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// One token per form+section. The SHA-256 hash is the validation path; the
/// raw token is also stored so the couple's links page can re-display the
/// active link. Revocation clears both. Regeneration overwrites both in place
/// (the old token dies atomically).
/// </summary>
public class SectionAccessToken : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public SectionType SectionType { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public string? RawToken { get; set; }

    public string CreatedByMembershipNo { get; set; } = string.Empty;

    public DateTime? RevokedAt { get; set; }
}
