namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

/// <summary>Bride's guardian (Waliyy) section.</summary>
public class GuardianSectionDetailDto
{
    public string Name { get; set; } = string.Empty;
    public string RelationToBride { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
}