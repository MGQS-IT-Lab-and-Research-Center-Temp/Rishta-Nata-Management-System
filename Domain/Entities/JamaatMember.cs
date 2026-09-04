using Domain.Abstractions;

namespace Domain.Entities;

public class JamaatMember : AuditableEntity
{
    public string Surname { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ChandaNo { get; set; } = string.Empty;
    public string? WasiyatNo { get; set; }
    public string? Title { get; set; }
    public string? AuxillaryBodyName { get; set; }
    public string? MiddleName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNo { get; set; } = string.Empty;
    public string JamaatName { get; set; } = string.Empty;
    public string CircuitName { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public string? MaritalStatus { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public string? Nationality { get; set; }

    // Roles come from the external Tajneed API, not any local role table
    // These are the role-name strings reported by the API login response
    // (Data.Roles) — see docs/stage-authorization-policy.md §3.2.
    public Guid? BrideGuardianId { get; set; }
    public BrideGuardian? BrideGuardian { get; set; }
}