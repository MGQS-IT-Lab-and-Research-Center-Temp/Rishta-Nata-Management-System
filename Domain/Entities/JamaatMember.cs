using Domain.Abstractions;

namespace Domain.Entities;

public class JamaatMember : AuditableEntity
{
    public string surname { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string chandaNo { get; set; } = string.Empty;
    public string? wasiyatNo { get; set; }
    public string? title { get; set; }
    public string? auxillaryBodyName { get; set; }
    public string? middleName { get; set; } = string.Empty!;
    public string? maidenName { get; set; }
    public DateTime dateOfBirth { get; set; }
    public string? phoneNo { get; set; } = string.Empty!;
    public string jamaatName { get; set; } = string.Empty;
    public string circuitName { get; set; } = string.Empty!;
    public string sex { get; set; } = string.Empty!;
    public string? maritalStatus { get; set; } = string.Empty!;
    public string? address { get; set; } = string.Empty!;
    public string? nextOfKinPhoneNo { get; set; } = string.Empty!;
    public string? nextOfKinName { get; set; } = string.Empty!;
    public string? nextOfKinAddress { get; set; } = string.Empty!;
    public string? nationality { get; set; }
    public string Password { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public string FullName => $"{firstName} {surname}".Trim();
    public bool IsSystemDefault { get; set; } = false;
    public string NewRole { get; set; } = string.Empty;
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiry { get; set; }
}
