using System;

namespace Infrastructure.DTOs.SharedSection;

/// <summary>
/// Fields a filler submits for one shared section. Guardian rows additionally
/// fill RelationToBride and AppointsRepresentative; MemberMembershipNo is
/// optional (member-prefill). RelationToBride is reused as the free-text
/// line for other sections (GroomWakeel: father's name; Representative:
/// "acting for").
/// </summary>
public class SectionFillData
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string RelationToBride { get; set; } = string.Empty;
    public bool AppointsRepresentative { get; set; }
    public string? MemberMembershipNo { get; set; }
    public bool IsMember { get; set; }
    public DateTime SignatureDate { get; set; } = DateTime.UtcNow;
}