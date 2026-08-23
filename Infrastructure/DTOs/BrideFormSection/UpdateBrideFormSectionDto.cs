namespace Infrastructure.DTOs.Bride;

public class UpdateBrideFormSectionDto
{
    public Guid MarriageApplicationFormId { get; set; }

    public string MembershipNo { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string ResidentOf { get; set; } = string.Empty;

    public string Genotype { get; set; } = string.Empty;

    public string BloodGroup { get; set; } = string.Empty;

    public string MaritalStatus { get; set; } = string.Empty;

    public decimal ProposedDowerAmount { get; set; }

    public decimal DowerAmountReceivedInCash { get; set; }

    public string SignatureTel { get; set; } = string.Empty;
}
