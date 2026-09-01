using Domain.Abstractions;

namespace Domain.Entities;

public class NikahCorrectionField : AuditableEntity
{
    public Guid NikahCorrectionRequestId { get; set; }
    public NikahCorrectionRequest NikahCorrectionRequest { get; set; } = null!;
    /// <summary>A stable application field key, not an arbitrary client-supplied property path.</summary>
    public string FieldKey { get; set; } = string.Empty;
}
