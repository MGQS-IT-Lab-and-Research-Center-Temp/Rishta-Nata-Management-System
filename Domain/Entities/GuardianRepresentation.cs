using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class GuardianRepresentation : AuditableEntity
{
    public Guid NikahApplicationId { get; set; }
    public NikahApplication NikahApplication { get; set; } = null!;
    public GuardianAttendanceOption AttendanceOption { get; set; }
    public string GuardianMembershipNo { get; set; } = string.Empty;
    public string GuardianName { get; set; } = string.Empty;
    public string RelationToBride { get; set; } = string.Empty;
    public string GuardianAddress { get; set; } = string.Empty;
    public string GuardianTelephone { get; set; } = string.Empty;
    public string? GuardianAttestedFullName { get; set; }
    public DateTime? GuardianAttestedAt { get; set; }
    public string? WakeelMembershipNo { get; set; }
    public string? WakeelName { get; set; }
    public string? WakeelAddress { get; set; }
    public string? WakeelActingFor { get; set; }
    public string? WakeelAttestedFullName { get; set; }
    public DateTime? WakeelAttestedAt { get; set; }
}
