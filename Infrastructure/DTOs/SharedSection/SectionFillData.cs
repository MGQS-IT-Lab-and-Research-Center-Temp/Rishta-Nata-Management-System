using System;

namespace Infrastructure.DTOs.SharedSection;

/// <summary>
/// Fields a filler submits for one shared section. Guardian rows additionally
/// fill RelationToBride; MemberMembershipNo is optional (member-prefill).
/// </summary>
public class SectionFillData
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string RelationToBride { get; set; } = string.Empty;
    public string? MemberMembershipNo { get; set; }
    public bool IsMember { get; set; }
    public DateTime SignatureDate { get; set; } = DateTime.UtcNow;
}