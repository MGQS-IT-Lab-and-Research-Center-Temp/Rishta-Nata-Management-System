using System.Collections.Generic;
using Presentation.ViewModels.JamaatMember;
using Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

namespace Presentation.ViewModels;

public class ReportsViewModel
{
    public int PendingApprovals { get; set; }

    public int ApprovedApplications { get; set; }

    public int RejectedApplications { get; set; }

    public int MarriedCouples { get; set; }

    public int TotalMembers { get; set; }

    public List<MarriedCoupleViewModel> MarriedCouplesList { get; set; } = new();

    public List<JamaatMemberVM> JamaatMembersList { get; set; } = new();
}
