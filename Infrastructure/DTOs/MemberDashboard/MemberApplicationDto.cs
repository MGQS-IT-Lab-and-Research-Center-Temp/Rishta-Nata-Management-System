using Domain.Enums;

namespace Infrastructure.DTOs.MemberDashboard;

public class MemberApplicationDto
{
    public Guid Id { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string SpouseName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime SubmittedDate { get; set; }

    public MarriageFormStage FormStage { get; set; }

    public bool IsActive { get; set; }

    public bool IsAwaitingYourSection { get; set; }
}
