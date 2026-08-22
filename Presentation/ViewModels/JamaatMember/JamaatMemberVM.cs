namespace Presentation.ViewModels.JamaatMember;

public class JamaatMemberVM
{
    public Guid Id { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ChandaNo { get; set; } = string.Empty;
    public string WasiyatNo { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string AuxillaryBodyName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty!;
    public string MaidenName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string PhoneNo { get; set; } = string.Empty!;
    public string JamaatName { get; set; } = string.Empty;
    public string CircuitName { get; set; } = string.Empty!;
    public string Sex { get; set; } = string.Empty!;
    public string MaritalStatus { get; set; } = string.Empty!;
    public string Address { get; set; } = string.Empty!;
    public string NextOfKinPhoneNo { get; set; } = string.Empty!;
    public string NextOfKinName { get; set; } = string.Empty!;
    public string NextOfKinAddress { get; set; } = string.Empty!;
    public string Nationality { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public string FullName => $"{FirstName} {Surname}".Trim();
    public bool IsSystemDefault { get; set; } = false;
    public string NewRole { get; set; } = string.Empty;
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiry { get; set; }
}
