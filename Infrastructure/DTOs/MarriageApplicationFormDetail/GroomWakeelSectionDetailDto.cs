namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

/// <summary>Groom's own Wakeel (representative) section — only populated
/// when the groom could not attend the Nikah in person.</summary>
public class GroomWakeelSectionDetailDto
{
    public string Name { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
}
