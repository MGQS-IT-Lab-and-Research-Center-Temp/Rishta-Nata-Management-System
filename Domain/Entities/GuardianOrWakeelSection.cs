using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class GuardianOrWakeelSection : AuditableEntity
    {
        public Guid MarriageApplicationFormId { get; set; }
        public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;
        public PartyType PartyType { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Tel { get; set; } = string.Empty;

        public string RelationToBride { get; set; } = string.Empty;

        public string? ActingFor { get; set; }

        public string? Signature { get; set; }

        public DateTime? Date { get; set; }

        public JamaatMember JamaatMember { get; set; } = null!;

    }
}
