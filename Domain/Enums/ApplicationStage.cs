namespace Domain.Enums
{
    public enum ApplicationStage
    {
        ApplicantsReview = 1,
        JamaatPresidentReview = 2,
        NationalRishtanataSecretaryVerification = 3,
        AmirApproval = 4,

        // Post-Amir, post-ceremony: the designated imam's sign-off. Appended so
        // existing stored ints are unaffected.
        ImamSignoff = 5
    }
}