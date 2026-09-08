namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

/// <summary>Representative (Wakeel) section.</summary>
public class RepresentativeSectionDetailDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ActingFor { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
}