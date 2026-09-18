namespace Infrastructure.DTOs.Imam;

public class ImamPendingApplicationDto
{
    public Guid FormId { get; set; }

    public Guid MarriageApplicationId { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string BrideName { get; set; } = string.Empty;

    public string BridegroomName { get; set; } = string.Empty;

    public DateTime? ApprovedDateOfNikah { get; set; }

    public string Venue { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;
}