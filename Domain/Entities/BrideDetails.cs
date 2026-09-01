using Domain.Abstractions;

namespace Domain.Entities;

public class BrideDetails : AuditableEntity
{
    public string MembershipNo { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string ResidentOf { get; set; } = string.Empty;
    public string Genotype { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string MaritalStatus { get; set; } = string.Empty;
    public decimal ProposedDowerAmount { get; set; }
    public decimal DowerAmountReceivedInCash { get; set; }
    public string FatherName { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string? AttestedFullName { get; set; }
    public DateTime? AttestedAt { get; set; }
}
