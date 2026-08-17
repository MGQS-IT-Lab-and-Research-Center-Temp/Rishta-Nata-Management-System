using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs.RishtanataSecretaryDashboardDto
{

    public class RecentActivityDto
    {
        public int Id { get; set; }
        public string? ActivityType { get; set; }
        public string? Description { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}
