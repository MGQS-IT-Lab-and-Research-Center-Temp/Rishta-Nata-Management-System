using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

/// <summary>
/// Section row created by the National Rishtanata Secretary with the
/// national-level recommendation. Maps to the "National Rishtanata Secretary"
/// section of the paper form.
/// </summary>
public class RishtanataRecommendationSection : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public string WakeelName { get; set; } = string.Empty;

    public string WakeelDeclaration { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;
}