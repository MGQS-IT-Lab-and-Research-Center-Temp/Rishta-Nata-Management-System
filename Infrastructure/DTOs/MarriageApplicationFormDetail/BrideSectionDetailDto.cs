using Domain.Enums;

namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

public class BrideSectionDetailDto
{
    public string MembershipNo { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string ResidentOf { get; set; } = string.Empty;
    public string Genotype { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public BrideMaritalStatus? MaritalStatus { get; set; }
    public decimal ProposedDowerAmount { get; set; }
    public decimal DowerAmountReceivedInCash { get; set; }
    public string SignatureTel { get; set; } = string.Empty;
}