using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs.RishtanataSecretaryDashboardDto
{
    public class MarriedCoupleDto
    {
        public Guid Id { get; set; }

        public Guid CertificateNumber { get; set; }

        public string? HusbandName { get; set; }

        public string? WifeName { get; set; }

        public string? JamaatName { get; set; }

        public DateTime MarriageDate { get; set; }

        public string? Status { get; set; }
    }
}
