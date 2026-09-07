using Domain.Abstractions;

namespace Domain.Entities;

public class BrideFormSection : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public string BrideMembershipNo { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public DateTime BrideDateOfBirth { get; set; }
    public string BrideResidentOf { get; set; } = string.Empty;
    public string BrideGenotype { get; set; } = string.Empty;
    public string BrideBloodGroup { get; set; } = string.Empty;
    public string BrideMaritalStatus { get; set; } = string.Empty;
    public decimal BrideProposedDowerAmount { get; set; }
    public decimal BrideDowerAmountReceivedInCash { get; set; }
    public string BrideSignatureTel { get; set; } = string.Empty;

    public string ReferenceNumber { get; set; } = string.Empty;
}
