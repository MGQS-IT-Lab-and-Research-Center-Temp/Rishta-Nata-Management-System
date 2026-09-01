namespace Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public Guid RecordId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string ChangeDetails { get; set; } = string.Empty;
    }
}
