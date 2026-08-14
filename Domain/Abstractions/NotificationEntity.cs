using Domain.Abstractions;

namespace Domain.Entities;

public class Notification : AuditableEntity
{
    public Guid RecipientId { get; set; }

    public Guid? MarriageApplicationId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }
}