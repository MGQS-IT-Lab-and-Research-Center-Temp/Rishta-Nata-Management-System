namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

public class AmirApprovalSectionDetailDto
{
    public DateTime? ApprovedDateOfNikah { get; set; }
    public string NationalAmirOrMissionarySignatureDate { get; set; } = string.Empty;
}