using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class NonAhmadiGuardian
    {
        public int Id { get; set; }
        public string? FullNmae { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public string? Religion { get; set; } 

    }
}
