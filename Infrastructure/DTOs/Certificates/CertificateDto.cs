namespace Infrastructure.DTOs.Certificates;

public class CertificateDto
{
    public Guid Id { get; set; }

    public string SerialNumber { get; set; } = string.Empty;

    public string BrideName { get; set; } = string.Empty;

    public string BridegroomName { get; set; } = string.Empty;

    public DateTime NikahDate { get; set; }

    public DateTime IssueDate { get; set; }

    public string? CertificateFilePath { get; set; }
}