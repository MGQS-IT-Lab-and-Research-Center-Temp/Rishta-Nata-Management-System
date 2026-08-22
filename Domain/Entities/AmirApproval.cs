using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Final approval by the National Amir / Missionary In-charge.
/// Maps to the "National Amir / Missionary In-charge" section of the paper form.
/// </summary>
public class AmirApproval : AuditableEntity
{
    // ===== Parent form =====
    public Guid MarriageApplicationFormId { get; set; }
    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    // ===== Paper form: National Amir / Missionary In-charge =====
    public DateTime? ApprovedDateOfNikah { get; set; }
    public string SignatureDate { get; set; } = string.Empty;
}