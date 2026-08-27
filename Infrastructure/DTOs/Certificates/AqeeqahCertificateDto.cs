namespace Infrastructure.DTOs.Certificates;

public class AqeeqahCertificateDto
{
    public Guid Id { get; set; }

    // Certificate
    public string SerialNumber { get; set; }

    // Child
    public string ChildName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; }

    public string PlaceOfBirth { get; set; }

    // Parents
    public string FatherName { get; set; }

    public string MotherName { get; set; }

    // Jamaat
    public Guid JamaatId { get; set; }

    public string JamaatName { get; set; }

    // Address
    public string Address { get; set; }

    // Administration
    public string OfficiatingMissionary { get; set; }

    public DateTime IssueDate { get; set; }

    public Guid IssuedByUserId { get; set; }

    // Aqeeqah
    public DateTime AqeeqahDate { get; set; }

    public string AqeeqahLocation { get; set; }

    public int AnimalCount { get; set; }

    // Certificate file
    public string? CertificateFilePath { get; set; }
}