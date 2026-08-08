using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum ApplicationStatus
    {
        Draft = 10,

        Submitted = 20,

        Reviewed1Approved = 30,
        Reviewed1Rejected = 40,

        Reviewed2Approved = 50,
        Reveiwed2Rejected = 60,

        Reviewed3Approved = 70,
        Reviewed3Rejected = 80
    }
}
