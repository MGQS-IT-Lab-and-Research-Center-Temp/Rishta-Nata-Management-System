using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class NikahWorkflowDecision : AuditableEntity
{
    public Guid NikahApplicationId { get; set; }
    public NikahApplication NikahApplication { get; set; } = null!;
    public NikahReviewStage Stage { get; set; }
    public NikahReviewOutcome Outcome { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime DecidedAt { get; set; }
}
