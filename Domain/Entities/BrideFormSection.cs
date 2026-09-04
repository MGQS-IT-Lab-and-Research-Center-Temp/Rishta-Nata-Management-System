using Domain.Abstractions;
using Domain.Entities;

public class BrideFormSection : AuditableEntity
{

    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;
}   