using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class FormApplication : AuditableEntity
{
    public ApplicationStatus Status { get; set; }
    public Guid MarriageApplicationFormId { get; set; }
    public MarriageApplicationForm MarriageApplicationForm { get; set; } = default!;
    //public Guid UserId { get; set; }
    //public User User { get; set; }
    public Guid CertificateId { get; set; }
    public Certificate Certificate { get; set; } = default!;
    public DateTime AppliedAt { get; set; }
}