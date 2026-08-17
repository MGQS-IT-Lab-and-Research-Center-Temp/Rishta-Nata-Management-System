using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.DTOs.FormApplication;

public class CreateFormApplicationDto
{
    public ApplicationStatus Status { get; set; }
    public Guid MarriageApplicationFormId { get; set; }
    public Guid CertificateId { get; set; }
    public DateTime AppliedAt { get; set; }

}