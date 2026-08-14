using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class NonAhmadiGuardian
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string Signature { get; set;}
        public Guid MarriageApplicationFormId { get; set; }
        public MarriageApplicationForm MarriageApplicationForm { get; set; }

        public string? Religion { get; set; } 

    }
}
