using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class NikahCorrectionRequest : AuditableEntity
{
    public Guid NikahApplicationId { get; set; }
    public NikahApplication NikahApplication { get; set; } = null!;
    public NikahReviewStage RequestedByStage { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
    public DateTime? ResubmittedAt { get; set; }
    public ICollection<NikahCorrectionField> Fields { get; set; } = new List<NikahCorrectionField>();
}
