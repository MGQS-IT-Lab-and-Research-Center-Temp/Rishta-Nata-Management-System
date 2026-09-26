namespace Infrastructure.DTOs.JamaatPresidentDashboardDto;

public class NikahApplicationDto
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = "";
    public string GroomName { get; set; } = "";
    public string BrideName { get; set; } = "";
    public string JamaatName { get; set; } = "";
    public DateTime SubmittedDate { get; set; }
    public string Status { get; set; } = "";

    // True when the viewing president is the one the current workflow stage is
    // waiting on (their own Jama'at's party). False when the item only appears
    // because another party belongs to their Jama'at.
    public bool IsActionableByMe { get; set; }

    // When not actionable: the name of the Jama'at whose president must review
    // first (the partner's Jama'at for the current stage). Empty when
    // actionable or when no president responsibility applies.
    public string AwaitingJamaatName { get; set; } = "";
}