using System;
using Domain.Abstractions;

namespace Domain.Entities;

public class Certificate : AuditableEntity
{
    // Required FK to MarriageApplication.
    public Guid MarriageApplicationId { get; set; }

    public MarriageApplication MarriageApplication { get; set; } = null!;

    // Unique certificate serial number.
    public string SerialNumber { get; set; } = null!;

    // Date the certificate was issued.
    public DateTime IssueDate { get; set; }

    // FK to ApplicationUser.
    public Guid? IssuedByUserId { get; set; }

    // Path to generated certificate file.
    public string? CertificateFilePath { get; set; }
}