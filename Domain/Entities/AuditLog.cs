using Domain.Abstractions;

namespace Domain.Entities
{
    public class AuditLog : AuditableEntity
    {
        public Guid UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public Guid RecordId { get; set; }
        public string ChangeDetails { get; set; } = string.Empty;
    }
}
