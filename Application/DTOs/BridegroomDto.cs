namespace Application.DTOs;

public class BridegroomDto
{
    public Guid MarriageApplicationId { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string MembershipNo { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string ResidentOf { get; set; } = string.Empty;

    public string Genotype { get; set; } = string.Empty;

    public string BloodGroup { get; set; } = string.Empty;

    public decimal DowerAmountPaidInCash { get; set; }

    public decimal DowerAmountToBePaid { get; set; }

    public bool IsFirstNikah { get; set; }

    public bool IsSecondThirdOrFourthNikah { get; set; }

    public bool FormerWifeIsDead { get; set; }

    public bool HasDivorcedFormerWife { get; set; }

    public bool FormerWifeIsPresent { get; set; }

    public bool FormerWifeObtainedKhula { get; set; }

    public string FatherName { get; set; } = string.Empty;

    public string SignatureTel { get; set; } = string.Empty;
}