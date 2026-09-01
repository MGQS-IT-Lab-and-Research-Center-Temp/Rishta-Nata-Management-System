using Domain.Abstractions;

namespace Domain.Entities;

public class BridegroomDetails : AuditableEntity
{
    public string MembershipNo { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string ResidentOf { get; set; } = string.Empty;
    public string Genotype { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public decimal DowerAmountPaidInCash { get; set; }
    public decimal DowerAmountToBePaid { get; set; }
    public bool IsFirstNikah { get; set; }
    public string? PreviousMarriageOutcome { get; set; }
    public string Telephone { get; set; } = string.Empty;
    public string? AttestedFullName { get; set; }
    public DateTime? AttestedAt { get; set; }
}
