namespace Domain.Enums
{
    public enum MarriageFormStage
    {
        AwaitingBride = 0,
        AwaitingBridegroom = 1,
        AwaitingWitnesses = 2,
        AwaitingImamVerification = 3,
        AwaitingJamaatPresident = 4,
        AwaitingRishtanataSecretary = 5,
        AwaitingAmirApproval = 6,
        Completed = 7,

        // Neutral start state: neither party has been fixed to go first.
        // Either the bride or the bridegroom may submit their section from
        // this stage; the first submission then advances to the partner's
        // "awaiting" stage. Appended after the existing values so existing
        // rows (and the stored ints) are unaffected.
        AwaitingApplicants = 8
    }
}
