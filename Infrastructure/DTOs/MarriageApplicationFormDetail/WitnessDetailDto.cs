namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

/// <summary>A single witness entry on the form.</summary>
public class WitnessDetailDto
{
    /// <summary>1 or 2 — the witness's position on the paper form.</summary>
    public int Position { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
}