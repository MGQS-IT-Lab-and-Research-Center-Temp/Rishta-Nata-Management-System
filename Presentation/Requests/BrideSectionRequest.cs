using System;
using Domain.Enums;

namespace Presentation.Requests;

public class BrideSectionRequest
{
    public Guid MarriageApplicationId { get; set; }
    public string BrideMembershipNo { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public DateTime BrideDateOfBirth { get; set; }
    public string BrideResidentOf { get; set; } = string.Empty;
    public string BrideGenotype { get; set; } = string.Empty;
    public string BrideBloodGroup { get; set; } = string.Empty;
    /// <summary>JSON: "Unmarried", "WidowedIddatComplete" or "DivorcedIddatComplete".</summary>
    public BrideMaritalStatus? BrideMaritalStatus { get; set; }
    public decimal BrideProposedDowerAmount { get; set; }
    public decimal BrideDowerAmountReceivedInCash { get; set; }
    public string BrideSignatureTel { get; set; } = string.Empty;
}
