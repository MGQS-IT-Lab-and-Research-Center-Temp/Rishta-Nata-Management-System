using Domain.Abstractions;

namespace Domain.Entities;

public class NikahCertificate : AuditableEntity
{
    public Guid NikahApplicationId { get; set; }
    public NikahApplication NikahApplication { get; set; } = null!;
    public string SerialNumber { get; set; } = string.Empty;
    public int Revision { get; set; } = 1;
    public Guid? ReplacesCertificateId { get; set; }
    public NikahCertificate? ReplacesCertificate { get; set; }
    public ICollection<NikahCertificate> ReplacementCertificates { get; set; } = new List<NikahCertificate>();
    public DateTime IssuedAt { get; set; }
    public Guid IssuedByUserId { get; set; }
    public string SnapshotJson { get; set; } = string.Empty;
    public string? PdfStorageKey { get; set; }
    public DateTime? SupersededAt { get; set; }
}
