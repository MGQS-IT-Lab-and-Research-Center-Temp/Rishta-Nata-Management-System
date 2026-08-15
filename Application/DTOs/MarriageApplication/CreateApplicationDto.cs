using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.MarriageApplication;

public class CreateApplicationDto
{
    public ApplicationStatus Status { get; set; }
    public Guid MarriageApplicationFormId { get; set; }
    //public MarriageApplicationFormDto MarriageApplicationForm { get; set; } = default!;
    //public Guid UserId { get; set; }
    //public User User { get; set; }
    public Guid CertificateId { get; set; }
    public CertificateDto Certificate { get; set; } = default!;
    public DateTime AppliedAt { get; set; }
}
