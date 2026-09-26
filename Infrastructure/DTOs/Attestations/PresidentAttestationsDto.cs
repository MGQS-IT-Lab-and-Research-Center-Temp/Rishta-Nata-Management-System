namespace Infrastructure.DTOs.Attestations;

/// <summary>
/// The Jama'at Presidents' recorded attestations (Gap 7). A null partner or
/// tick means "not recorded yet" (or signed before Gap 7).
/// </summary>
public class PresidentAttestationsDto
{
    public PartnerAttestationDto? Bride { get; set; }

    public PartnerAttestationDto? Groom { get; set; }

    public bool? GuardianIsBonafide { get; set; }

    public bool? BrideSignedFreely { get; set; }
}
