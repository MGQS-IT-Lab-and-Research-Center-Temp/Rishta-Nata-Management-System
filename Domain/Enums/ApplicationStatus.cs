namespace Domain.Enums
{
    public enum ApplicationStatus
    {
        Draft = 10,

        Submitted = 20,

        // ==============================
        // Jama'at President
        // ==============================

        JamaatPresidentReviewApproved = 30,

        JamaatPresidentPendingApproval = 35,

        JamaatPresidentReviewRejected = 40,

        JamaatPresidentInformationRequired = 45,

        // ==============================
        // National Rishtanata Secretary
        // ==============================

        NationalRishtanataSecretaryReviewApproved = 50,

        NationalRishtanataSecretaryPendingApproval = 55,

        NationalRishtanataSecretaryReviewRejected = 60,

        // ==============================
        // Amir
        // ==============================

        AmirReviewApproved = 70,

        AmirReviewRejected = 80
    }
}