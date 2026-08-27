namespace Infrastructure.DTOs.JamaatPresidentDashboardDto;

public class RecentActivityDto
{
    public string? ApplicationNumber { get; set; }
    public string Description { get; set; } = "";
    public DateTime Date { get; set; }
}