using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs.RishtanataSecretaryDashboardDto
{
    public class RishtanataSecretaryDashboardDto
    {
        public string? SecretaryName { get; set; }

        public int PendingApprovals { get; set; }

        public int ApprovedApplications { get; set; }

        public int RejectedApplications { get; set; }

        public int MarriedCouples { get; set; }

        public int TotalMembers { get; set; }

        public List<PendingApprovalDto> PendingApplications { get; set; }
            = new();

        public List<RecentActivityDto> RecentActivities { get; set; }
            = new();
    }

}
