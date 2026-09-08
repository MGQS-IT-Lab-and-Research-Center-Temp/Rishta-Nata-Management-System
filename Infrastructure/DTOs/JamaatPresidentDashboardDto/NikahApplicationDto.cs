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
}