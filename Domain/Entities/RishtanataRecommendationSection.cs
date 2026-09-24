using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Section row created by the National Rishtanata Secretary with the
/// national-level recommendation. Maps to the "National Rishtanata Secretary"
/// section of the paper form. Name and SignatureDate are mirrored onto
/// MarriageApplicationForm.NationalRishtanataSecretaryName / …SignatureDate.
/// </summary>
public class RishtanataRecommendationSection : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    /// <summary>The National Rishtanata Secretary's name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The secretary's national-level recommendation text.</summary>
    public string Recommendation { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;

    /// <summary>
    /// ChandaNo of the Imam the office designates to officiate the ceremony. The
    /// designated imam is the ONLY person who may sign off (AwaitingImamSignoff).
    /// </summary>
    public string OfficiatingImamMembershipNo { get; set; } = string.Empty;
}