using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Verification by the local Jamaat (branch) President.
/// Maps to the "Jamaat President" section of the paper form.
/// </summary>
public class JamaatPresidentVerification : AuditableEntity
{
    // ===== Parent form =====
    public Guid MarriageApplicationFormId { get; set; }
    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    // ===== Paper form: Jamaat President =====
    public string Name { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
}