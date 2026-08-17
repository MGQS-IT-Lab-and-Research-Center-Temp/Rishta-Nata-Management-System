

namespace Infrastructure.DTOs.FormApplication;

public class CertificateDto
{
    public Guid Id { get; set; }
    public DateTime IssueDate { get; set; }
    public string? CertificateFilePath { get; set; }
}
