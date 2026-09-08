using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

/// <summary>
/// Section row created by the Jamaat (branch) President during local
/// verification. Maps to the "Jamaat President" section of the paper form.
/// </summary>
public class JamaatPresidentVerificationSection : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public string Tel { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;
}