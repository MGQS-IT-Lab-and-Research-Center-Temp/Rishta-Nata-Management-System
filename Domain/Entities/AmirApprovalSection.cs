using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

/// <summary>
/// Section row created when the National Amir / Missionary In-charge gives
/// final approval. Maps to the "National Amir / Missionary In-charge" section
/// of the paper form.
/// </summary>
public class AmirApprovalSection : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public DateTime? ApprovedDateOfNikah { get; set; }

    public string SignatureDate { get; set; } = string.Empty;
}