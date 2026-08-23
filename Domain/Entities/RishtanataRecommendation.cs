using Domain.Abstractions;

namespace Domain.Entities;

public class RishtanataRecommendation : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public string WakeelName { get; set; } = string.Empty;

    public string WakeelDeclaration { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;
}