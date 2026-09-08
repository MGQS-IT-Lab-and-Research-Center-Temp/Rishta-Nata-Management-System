namespace Infrastructure.DTOs.BrideGuardian;

public class BrideGuardianDto
{
    public Guid BrideGuardianId { get; set; }
    public Guid MarriageApplicationId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public List<Guid> BrideIds { get; set; } = new();
    public string GuardianName { get; set; } = string.Empty;
    public string GuardianRelationToBride { get; set; } = string.Empty;
    public string GuardianAddress { get; set; } = string.Empty;
    public string GuardianTel { get; set; } = string.Empty;
    public string GuardianSignatureDate { get; set; } = string.Empty;
}
