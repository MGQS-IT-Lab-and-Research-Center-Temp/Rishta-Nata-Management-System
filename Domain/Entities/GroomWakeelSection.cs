using Domain.Abstractions;

namespace Domain.Entities
{
    public class GroomWakeelSection : AuditableEntity
    {
        public Guid MarriageApplicationFormId { get; set; }

        public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string? Signature { get; set; }
        public DateTime? Date { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}
