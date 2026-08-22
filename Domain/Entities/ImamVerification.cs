using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Verification by the Officiating Imam / Missionary who conducted the Nikah ceremony.
/// Maps to the "Officiating Imam / Missionary" section of the paper form.
/// </summary>
public class ImamVerification : AuditableEntity
{
    // ===== Parent form =====
    public Guid MarriageApplicationFormId { get; set; }
    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    // ===== Paper form: Officiating Imam / Missionary =====
    public string Name { get; set; } = string.Empty;
    public string AddressJamaat { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
}