using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum ApplicationStatus
    {
        Draft = 10,

        Submitted = 20,

        JamaatPresidentReviewApproved = 30,
        jamaatPresidentPendingApproval = 35,
        JamaatPresidentReviewRejected = 40,

        NationalRishtanataSecretaryReviewApproved = 50,
        NationalRishtanataSecretaryPendingApproval = 55,
        NationalRishtanataSecretaryReveiwRejected = 60,

        AmirReviewApproved = 70,
        AmirReviewRejected = 80
    }
}
