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
}