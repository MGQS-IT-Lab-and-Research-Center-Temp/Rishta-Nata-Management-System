using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.MarriageApplication;

public class CreateApplicationDto
{
    public Guid MarriageApplicationFormId { get; set; }

    public Guid CertificateId { get; set; }
}