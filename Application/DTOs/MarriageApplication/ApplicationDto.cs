using Domain.Enums;

namespace Application.DTOs.MarriageApplication;

public class ApplicationDto
{
    public Guid Id { get; set; }

    public ApplicationStatus Status { get; set; }

    public Guid MarriageApplicationFormId { get; set; }

    public Guid CertificateId { get; set; }

    public CertificateDto? Certificate { get; set; }

    public DateTime AppliedAt { get; set; }
}