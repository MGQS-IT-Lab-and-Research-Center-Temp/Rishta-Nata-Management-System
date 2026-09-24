using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// The uploaded divorce certificate for one party on a form (Gap 8): Khula
/// for a divorced bride, Talaq for a divorced groom. One row per form+party;
/// a new upload overwrites the row and the old file is deleted. The file
/// itself lives in IDocumentStorage under StoredFileName.
/// </summary>
public class DivorceEvidenceDocument : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public DivorceEvidenceParty Party { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    public DateTime UploadedAt { get; set; }

    /// <summary>Membership number of the member who uploaded the file.</summary>
    public string UploadedBy { get; set; } = string.Empty;
}
