namespace Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

public class RishtanataSecretaryDashboardViewModel
{
    public string? SecretaryName { get; set; }

    public int PendingApprovals { get; set; }

    public int ApprovedApplications { get; set; }

    public int RejectedApplications { get; set; }

    public int MarriedCouples { get; set; }

    public int TotalMembers { get; set; }

    public List<PendingApprovalViewModel> PendingApplications { get; set; }
        = new();

    public List<RecentActivityViewModel> RecentActivities { get; set; }
        = new();
}
