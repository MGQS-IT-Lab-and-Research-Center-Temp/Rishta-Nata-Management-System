using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class MarriageApplication : AuditableEntity
    {
        public ApplicationStatus Status { get; set; }

        public Guid UserId { get; set; }

        public Certificate? Certificate { get; set; }

        public string? SerialNumber { get; set; }

        // One MarriageApplication has one MarriageApplicationForm
        public MarriageApplicationForm? MarriageApplicationForm { get; set; }
    }
}