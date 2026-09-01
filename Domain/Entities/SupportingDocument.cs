using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class SupportingDocument : AuditableEntity
{
    public Guid NikahApplicationId { get; set; }
    public NikahApplication NikahApplication { get; set; } = null!;
    public SupportingDocumentType Type { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public bool IsVerified { get; set; }
}
