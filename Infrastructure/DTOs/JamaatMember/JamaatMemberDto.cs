namespace Infrastructure.DTOs.JamaatMember;

public class JamaatMemberDto
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
    public List<Guid> RoleIds { get; set; } = new();
    public string FullName => $"{FirstName} {Surname}".Trim();
    public bool IsSystemDefault { get; set; } = false;
    public string NewRole { get; set; } = string.Empty;
    public string MemberNumber { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
}