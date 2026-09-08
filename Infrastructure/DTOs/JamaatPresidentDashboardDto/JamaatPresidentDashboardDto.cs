namespace Infrastructure.DTOs.JamaatPresidentDashboardDto;

public class JamaatPresidentDashboardDto
{
    public string PresidentName { get; set; } = "";
    public string JamaatName { get; set; } = "";
    public string CircuitName { get; set; } = "";

    public int PendingNikahReviews { get; set; }
    public int ReviewedToday { get; set; }
    public int TotalNikahApplications { get; set; }

    public List<NikahApplicationDto> PendingApplications { get; set; } = new();

    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}