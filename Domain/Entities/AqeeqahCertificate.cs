using Domain.Abstractions;

namespace Domain.Entities;

public class AqeeqahCertificate : AuditableEntity
{
    // Certificate
    public string SerialNumber { get; set; } = string.Empty;

    // Child Information
    public string ChildName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string PlaceOfBirth { get; set; } = string.Empty;

    // Parents
    public string FatherName { get; set; } = string.Empty;

    public string MotherName { get; set; } = string.Empty;

    // Jamaat
    public Guid JamaatId { get; set; }

    public string JamaatName { get; set; } = string.Empty;

    // Address
    public string Address { get; set; } = string.Empty;

    // Administration
    public string OfficiatingMissionary { get; set; }

    public DateTime IssueDate { get; set; }

    public Guid IssuedByUserId { get; set; }

    // Aqeeqah
    public DateTime AqeeqahDate { get; set; }

    public string AqeeqahLocation { get; set; } = string.Empty;

    public int AnimalCount { get; set; }

    // Certificate file
    public string? CertificateFilePath { get; set; }
}