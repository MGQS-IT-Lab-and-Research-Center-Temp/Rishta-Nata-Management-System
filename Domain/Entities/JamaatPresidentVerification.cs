using Domain.Abstractions;

namespace Domain.Entities;    
public class JamaatPresidentVerification : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }
    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string SignatureDate { get; set; } = string.Empty;
    public string Tel {get; set;} = string.Empty;
}