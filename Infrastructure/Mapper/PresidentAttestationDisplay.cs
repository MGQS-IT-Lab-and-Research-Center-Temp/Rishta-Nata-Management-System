using Domain.Entities;
using Infrastructure.DTOs.Attestations;

namespace Infrastructure.Mapper;

/// <summary>
/// Builds the recorded president attestations (Gap 7) from the two president
/// sections. The groom's attestation comes from the groom's president when one
/// signed; on the same-Jama'at path the bride's president attested the groom
/// too, so it falls back to the bride section. Callers must load
/// JamaatPresidentVerification and GroomJamaatPresidentVerification.
/// </summary>
public static class PresidentAttestationDisplay
{
    public static PresidentAttestationsDto? From(MarriageApplicationForm form)
    {
        var brideSide = form.JamaatPresidentVerification;
        var groomSide = form.GroomJamaatPresidentVerification;

        var bride = Partner(brideSide?.BrideIsBornAhmadi, brideSide?.BrideYearsAsAhmadi, brideSide?.BrideMarriageReason);

        var groom = Partner(groomSide?.GroomIsBornAhmadi, groomSide?.GroomYearsAsAhmadi, groomSide?.GroomMarriageReason)
            ?? Partner(brideSide?.GroomIsBornAhmadi, brideSide?.GroomYearsAsAhmadi, brideSide?.GroomMarriageReason);

        if (bride is null && groom is null)
        {
            return null;
        }

        // The ticks are only meaningful once the bride's president has attested.
        return new PresidentAttestationsDto
        {
            Bride = bride,
            Groom = groom,
            GuardianIsBonafide = bride is null ? null : brideSide!.GuardianIsBonafide,
            BrideSignedFreely = bride is null ? null : brideSide!.BrideSignedFreely
        };
    }

    public static PresidentAttestationsDto FromOrEmpty(MarriageApplicationForm form) =>
        From(form) ?? new PresidentAttestationsDto();

    private static PartnerAttestationDto? Partner(bool? isBornAhmadi, int? yearsAsAhmadi, string? marriageReason) =>
        isBornAhmadi is null
            ? null
            : new PartnerAttestationDto
            {
                IsBornAhmadi = isBornAhmadi.Value,
                YearsAsAhmadi = yearsAsAhmadi,
                MarriageReason = marriageReason ?? string.Empty
            };
}
