using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs.RishtanataSecretaryDashboardDto
{
    public class JamaatMemberDto
    {
        public Guid Id { get; set; }

        public string? MembershipNumber { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? JamaatName { get; set; }

        public string? MaritalStatus { get; set; }

        public string? Occupation { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
