using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// National-level recommendation by the Rishtanata Secretary.
/// Maps to the "National Rishtanata Secretary" section of the paper form.
/// </summary>
public class RishtanataRecommendation : AuditableEntity
{
    // ===== Parent form =====
    public Guid MarriageApplicationFormId { get; set; }
    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    // ===== Paper form: National Rishtanata Secretary =====
    public string Name { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
}