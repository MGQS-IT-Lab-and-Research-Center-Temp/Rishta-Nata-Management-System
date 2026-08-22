namespace Infrastructure.DTOs.Certificates;

public class AqeeqahCertificateDto
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string ChildName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string MotherName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime AqeeqahDate { get; set; }
    public string? AqeeqahLocation { get; set; }
    public int? AnimalCount { get; set; }
    public DateTime IssueDate { get; set; }
    public string? CertificateFilePath { get; set; }
}
