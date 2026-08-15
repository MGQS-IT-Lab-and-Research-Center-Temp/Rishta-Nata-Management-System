using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.MarriageApplication;

public class CreateApplicationDto
{
    public ApplicationStatus Status { get; set; }
    public Guid MarriageApplicationFormId { get; set; }
    public Guid CertificateId { get; set; }
    public DateTime AppliedAt { get; set; }
}