using Domain.Abstractions;

namespace Domain.Entities;

public class AqeeqahCertificate : AuditableEntity
{
    // Certificate number/serial number
    public string SerialNumber { get; set; } = string.Empty;

    // Child Information
    public string ChildName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string MotherName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty; // Male or Female

    // Aqeeqah Details
    public DateTime AqeeqahDate { get; set; }
    public string? AqeeqahLocation { get; set; }
    public int? AnimalCount { get; set; } // Number of animals sacrificed

    // Certificate Administration
    public DateTime IssueDate { get; set; }
    public Guid IssuedByUserId { get; set; }
    public Guid JamaatId { get; set; }

    // Certificate File
    public string? CertificateFilePath { get; set; }
}
