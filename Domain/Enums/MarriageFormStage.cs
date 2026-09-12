namespace Domain.Enums
{
    public enum MarriageFormStage
    {
        AwaitingBride = 0,
        AwaitingBridegroom = 1,
        AwaitingWitnesses = 2,

        // Obsolete: kept so stored integers remain meaningful, but this stage is
        // unreachable — no authorization path, no write path, no advance into it.
        // The Imam now signs off AFTER the ceremony (AwaitingImamSignoff), not
        // during the filling phase.
        AwaitingImamVerification = 3,

        // The first president sign-off: the bride's Jama'at President. When both
        // partners share a Jama'at this president also signs for the groom, and
        // the AwaitingGroomJamaatPresident stage is skipped entirely.
        AwaitingBrideJamaatPresident = 4,

        AwaitingRishtanataSecretary = 5,
        AwaitingAmirApproval = 6,
        Completed = 7,

        // Neutral start state: neither party has been fixed to go first.
        // Either the bride or the bridegroom may submit their section from
        // this stage; the first submission then advances to the partner's
        // "awaiting" stage. Appended after the existing values so existing
        // rows (and the stored ints) are unaffected.
        AwaitingApplicants = 8,

        // The groom's Jama'at President sign-off. Only reachable when the
        // partners come from different Jama'ats; skipped entirely when they share
        // one. Appended so existing stored ints are unaffected.
        AwaitingGroomJamaatPresident = 9,

        // Post-ceremony: the Imam designated by the National Rishtanata office
        // signs that the ceremony took place on the approved date/venue. The
        // workflow advances to Completed after this step. Appended so existing
        // stored ints are unaffected.
        AwaitingImamSignoff = 10
    }
}