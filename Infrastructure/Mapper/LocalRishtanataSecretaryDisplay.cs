using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Mapper;

/// <summary>
/// Resolves which Local Rishtanata Secretary entry to show for the groom's
/// side (Gap 4). When the partners share a Jama'at, the bride's president signs
/// for both and the groom stage is skipped, so the bride-side entry covers the
/// groom's side too.
/// </summary>
public static class LocalRishtanataSecretaryDisplay
{
    /// <summary>
    /// True on the same-Jama'at path: the bride side is recorded, the groom
    /// side isn't, and the form isn't waiting on a separate groom's president.
    /// Keyed on the local-secretary mirrors (required on every signature since
    /// Gap 4), not the president's own mirrors.
    /// </summary>
    public static bool GroomSideCoveredByBrideSide(MarriageApplicationForm form) =>
        !string.IsNullOrWhiteSpace(form.BrideLocalRishtanataSecretarySignatureDate) &&
        string.IsNullOrWhiteSpace(form.GroomLocalRishtanataSecretarySignatureDate) &&
        form.FormStage != MarriageFormStage.AwaitingGroomJamaatPresident;

    public static string GroomSideName(MarriageApplicationForm form) =>
        GroomSideCoveredByBrideSide(form)
            ? form.BrideLocalRishtanataSecretaryName
            : form.GroomLocalRishtanataSecretaryName;

    public static string GroomSideSignatureDate(MarriageApplicationForm form) =>
        GroomSideCoveredByBrideSide(form)
            ? form.BrideLocalRishtanataSecretarySignatureDate
            : form.GroomLocalRishtanataSecretarySignatureDate;
}
