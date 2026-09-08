using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// One token per form+section. Only the SHA-256 hash is stored; the raw value
/// is shown exactly once at mint/regenerate time. Regeneration overwrites the
/// hash in place (the old token dies atomically).
/// </summary>
public class SectionAccessToken : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public SectionType SectionType { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public string CreatedByMembershipNo { get; set; } = string.Empty;

    public DateTime? RevokedAt { get; set; }
}