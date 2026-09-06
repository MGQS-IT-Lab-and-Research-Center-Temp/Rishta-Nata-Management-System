namespace Infrastructure.DTOs.Members;

public class MemberLookupDto
{
    public string ChandaNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string JamaatName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
}
