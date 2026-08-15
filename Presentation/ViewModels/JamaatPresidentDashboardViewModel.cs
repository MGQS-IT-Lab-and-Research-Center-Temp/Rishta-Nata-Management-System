namespace Presentation.ViewModels;

public class JamaatPresidentDashboardViewModel
{
    public string PresidentName { get; set; } = "";
    public string JamaatName { get; set; } = "";
    public string CircuitName { get; set; } = "";

    public int PendingNikahReviews { get; set; }
    public int ReviewedToday { get; set; }
    public int TotalNikahApplications { get; set; }

    public List<NikahApplicationViewModel> PendingApplications { get; set; } = new();

    public List<RecentActivityViewModel> RecentActivities { get; set; } = new();
}


public class NikahApplicationViewModel
{
    public Guid Id { get; set; }

    public string ReferenceNumber { get; set; } = "";

    public string GroomName { get; set; } = "";

    public string BrideName { get; set; } = "";

    public string JamaatName { get; set; } = "";

    public DateTime SubmittedDate { get; set; }

    public string Status { get; set; } = "";
}


public class RecentActivityViewModel
{
    public string? ApplicationNumber { get; set; }

    public string Description { get; set; } = "";

    public DateTime Date { get; set; }
}