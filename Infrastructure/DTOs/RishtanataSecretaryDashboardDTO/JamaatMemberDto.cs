using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs.RishtanataSecretaryDashboardDTO
{

    public class JamaatMemberDto
    {
        public int Id { get; set; }

        public string? MemberNumber { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? JamaatName { get; set; }

        public string? MaritalStatus { get; set; }

        public string? Occupation { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
