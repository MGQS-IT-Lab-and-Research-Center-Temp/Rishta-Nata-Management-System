using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class Witness  : AuditableEntity
{
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;

    public WitnessRole Role { get; set; }

    public int WitnessNumber { get; set; }

    public string InvitationToken { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }
}