namespace Domain.Enums;

/// <summary>
/// The lifecycle of a Nikah application. A correction retains the review stage
/// that requested it; it is not a terminal rejection.
/// </summary>
public enum NikahApplicationStatus
{
    Draft = 0,
    AwaitingContributorConfirmations = 1,
    AwaitingJamaatPresidentReview = 2,
    AwaitingNationalReview = 3,
    AwaitingAmirApproval = 4,
    CorrectionRequested = 5,
    Approved = 6,
    CertificateIssued = 7,
    Archived = 8
}
