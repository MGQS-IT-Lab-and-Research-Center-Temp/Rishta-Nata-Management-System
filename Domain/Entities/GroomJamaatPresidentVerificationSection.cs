using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Section row created by the Groom's Jama'at President when the partners come
/// from different Jama'ats. Skipped entirely when both partners share a Jama'at
/// (in that case the bride's president signs for both).
/// Also carries the groom-side (F1) Local Rishtanata Secretary block (Gap 4).
/// </summary>
public class GroomJamaatPresidentVerificationSection : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string Tel { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;

    public string LocalRishtanataSecretaryName { get; set; } = string.Empty;

    public string LocalRishtanataSecretaryTel { get; set; } = string.Empty;

    public string LocalRishtanataSecretarySignatureDate { get; set; } = string.Empty;
}