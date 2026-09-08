using Domain.Enums;

namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

/// <summary>One entry in a form's rejection/revert audit trail.</summary>
public class RejectionHistoryItemDto
{
    public Guid Id { get; set; }
    public ApplicationStage RejectedAtStage { get; set; }
    public ApplicationStage RevertedToStage { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
}