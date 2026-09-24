using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

/// <summary>
/// Section row created by the Jamaat (branch) President during local
/// verification. Maps to the "Jamaat President" section of the paper form.
/// Also carries the bride-side (F2) Local Rishtanata Secretary block, which the
/// president records when signing (Gap 4). When the partners share a Jama'at
/// this one entry covers both sides.
/// </summary>
public class JamaatPresidentVerificationSection : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string Tel { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;

    public string LocalRishtanataSecretaryName { get; set; } = string.Empty;

    public string LocalRishtanataSecretaryTel { get; set; } = string.Empty;

    public string LocalRishtanataSecretarySignatureDate { get; set; } = string.Empty;

    // President's attestations (Gap 7). Null = not recorded (signed before Gap 7).
    public bool? BrideIsBornAhmadi { get; set; }

    /// <summary>Null when the bride is a born Ahmadi.</summary>
    public int? BrideYearsAsAhmadi { get; set; }

    public string BrideMarriageReason { get; set; } = string.Empty;

    /// <summary>Only recorded when the partners share a Jama'at (this president signs for both).</summary>
    public bool? GroomIsBornAhmadi { get; set; }

    public int? GroomYearsAsAhmadi { get; set; }

    public string GroomMarriageReason { get; set; } = string.Empty;

    public bool GuardianIsBonafide { get; set; }

    public bool BrideSignedFreely { get; set; }
}