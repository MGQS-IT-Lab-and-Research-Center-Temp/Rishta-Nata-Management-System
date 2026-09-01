using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class NikahWitnessAttestation : AuditableEntity
{
    public Guid NikahApplicationId { get; set; }
    public NikahApplication NikahApplication { get; set; } = null!;
    public NikahWitnessRole Role { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string InvitationTokenHash { get; set; } = string.Empty;
    public DateTime? InvitationExpiresAt { get; set; }
    public string? AttestedFullName { get; set; }
    public DateTime? AttestedAt { get; set; }
}
