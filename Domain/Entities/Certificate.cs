using Domain.Abstractions;

namespace Domain.Entities;

public class Certificate : AuditableEntity
{
    // Certificate number printed on the certificate.
    public string SerialNumber { get; set; } = string.Empty;

    // Bride
    public string BrideName { get; set; } = string.Empty;
    public string BrideFatherName { get; set; } = string.Empty;
    public string BrideResidentOf { get; set; } = string.Empty;

    // Bridegroom
    public string BridegroomName { get; set; } = string.Empty;
    public string BridegroomFatherName { get; set; } = string.Empty;
    public string BridegroomResidentOf { get; set; } = string.Empty;

    // Marriage
    public DateTime NikahDate { get; set; }
    public decimal DowryAmount { get; set; }

    // Application relationship
    public Guid MarriageApplicationId { get; set; }
    public Application MarriageApplication { get; set; } = null!;

    // Certificate administration
    public DateTime IssueDate { get; set; }

    public Guid IssuedByUserId { get; set; }

    public string? CertificateFilePath { get; set; }
}