namespace Presentation.ViewModels;

public class PendingApprovalViewModel
{
    public Guid Id { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public string GroomName { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public string JamaatName { get; set; } = string.Empty;
    public string PresidentName { get; set; } = string.Empty;
    public DateTime SubmittedDate { get; set; }
    public string PresidentRecommendation { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
