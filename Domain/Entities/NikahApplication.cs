using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// The aggregate root for the prescribed Nikah application.
/// It deliberately models the official form directly instead of acting as a
/// generic form-definition engine.
/// </summary>
public class NikahApplication : AuditableEntity
{
    public Guid PrimaryApplicantUserId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public NikahApplicationStatus Status { get; private set; } = NikahApplicationStatus.Draft;
    public NikahReviewStage? AwaitingReviewStage { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public DateTime ProposedNikahDate { get; set; }
    public string Venue { get; set; } = string.Empty;

    public BrideDetails Bride { get; set; } = new();
    public BridegroomDetails Bridegroom { get; set; } = new();
    public GuardianRepresentation GuardianRepresentation { get; set; } = new();
    public ICollection<NikahWitnessAttestation> Witnesses { get; set; } = new List<NikahWitnessAttestation>();
    public ICollection<SupportingDocument> Documents { get; set; } = new List<SupportingDocument>();
    public ICollection<NikahWorkflowDecision> WorkflowDecisions { get; set; } = new List<NikahWorkflowDecision>();
    public ICollection<NikahCorrectionRequest> CorrectionRequests { get; set; } = new List<NikahCorrectionRequest>();
    public ICollection<NikahCertificate> Certificates { get; set; } = new List<NikahCertificate>();

    public void Submit(DateTime submittedAt)
    {
        SubmittedAt = submittedAt;
        Status = NikahApplicationStatus.AwaitingJamaatPresidentReview;
        AwaitingReviewStage = NikahReviewStage.JamaatPresident;
    }

    public void AdvanceAfterApproval(NikahReviewStage approvedStage, DateTime approvedAt)
    {
        if (AwaitingReviewStage != approvedStage)
            throw new InvalidOperationException("The application is not awaiting this review stage.");

        switch (approvedStage)
        {
            case NikahReviewStage.JamaatPresident:
                Status = NikahApplicationStatus.AwaitingNationalReview;
                AwaitingReviewStage = NikahReviewStage.NationalRishtanata;
                break;
            case NikahReviewStage.NationalRishtanata:
                Status = NikahApplicationStatus.AwaitingAmirApproval;
                AwaitingReviewStage = NikahReviewStage.Amir;
                break;
            case NikahReviewStage.Amir:
                Status = NikahApplicationStatus.Approved;
                AwaitingReviewStage = null;
                ApprovedAt = approvedAt;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(approvedStage));
        }
    }

    public void RequestCorrection(NikahReviewStage requestingStage)
    {
        if (AwaitingReviewStage != requestingStage)
            throw new InvalidOperationException("Only the current review stage can request a correction.");

        Status = NikahApplicationStatus.CorrectionRequested;
    }

    public void ResubmitCorrection()
    {
        if (Status != NikahApplicationStatus.CorrectionRequested || AwaitingReviewStage is null)
            throw new InvalidOperationException("The application is not awaiting a correction.");

        Status = AwaitingReviewStage.Value switch
        {
            NikahReviewStage.JamaatPresident => NikahApplicationStatus.AwaitingJamaatPresidentReview,
            NikahReviewStage.NationalRishtanata => NikahApplicationStatus.AwaitingNationalReview,
            NikahReviewStage.Amir => NikahApplicationStatus.AwaitingAmirApproval,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void MarkCertificateIssued() => Status = NikahApplicationStatus.CertificateIssued;
}
