using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs.RishtanataSecretaryDashboardDTO
{


    public class PendingApprovalDto
    {
        public Guid Id { get; set; }

        public string? ApplicationNumber { get; set; }

        public string? GroomName { get; set; }

        public string? BrideName { get; set; }

        public string? JamaatName { get; set; }

        public string? PresidentName { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string? PresidentRecommendation { get; set; }

        public string? Status { get; set; }
    }


}
