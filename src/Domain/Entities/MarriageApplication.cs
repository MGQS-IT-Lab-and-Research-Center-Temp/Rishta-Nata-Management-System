using RishtaNata.Domain.Enums;

namespace RishtaNata.Domain.Entities;

public class MarriageApplication : AuditableEntity
{
    public ApplicationStatus Status { get; set; }

    public string ApplicationUserId { get; set; } = null!;

    public string? NikahSerialNumber { get; set; }
}