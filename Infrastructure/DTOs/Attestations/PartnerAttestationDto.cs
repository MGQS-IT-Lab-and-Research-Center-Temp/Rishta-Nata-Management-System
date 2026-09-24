namespace Infrastructure.DTOs.Attestations;

/// <summary>A recorded president's attestation for one partner (Gap 7).</summary>
public class PartnerAttestationDto
{
    public bool IsBornAhmadi { get; set; }

    /// <summary>Null when born Ahmadi.</summary>
    public int? YearsAsAhmadi { get; set; }

    public string MarriageReason { get; set; } = string.Empty;
}
