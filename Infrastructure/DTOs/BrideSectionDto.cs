using Domain.Enums;

namespace Infrastructure.DTOs;

public class BrideSectionDto
{
    public Guid MarriageApplicationId { get; set; }
    public string BrideMembershipNo { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public DateTime BrideDateOfBirth { get; set; }
    public string BrideResidentOf { get; set; } = string.Empty;
    public string BrideGenotype { get; set; } = string.Empty;
    public string BrideBloodGroup { get; set; } = string.Empty;
    public BrideMaritalStatus? BrideMaritalStatus { get; set; }
    public string BrideDivorceEvidence { get; set; } = string.Empty;
    public decimal BrideProposedDowerAmount { get; set; }
    public decimal BrideDowerAmountReceivedInCash { get; set; }
    public string BrideSignatureTel { get; set; } = string.Empty;
}