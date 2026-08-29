using Domain.Abstractions;

namespace Domain.Entities;

public class JamaatMemberRole : AuditableEntity
{
    public Guid JamaatMemberId { get; set; }
    public JamaatMember JamaatMember { get; set; } = default!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string? AssignedBy { get; set; }
}