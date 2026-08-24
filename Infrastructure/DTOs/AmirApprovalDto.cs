namespace Infrastructure.DTOs;

public class AmirApprovalDto
{
    public Guid MarriageApplicationId { get; set; }
    public bool IsApproved { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime? ApprovedDateOfNikah { get; set; }
    public string NationalAmirOrMissionarySignatureDate { get; set; } = string.Empty;
}