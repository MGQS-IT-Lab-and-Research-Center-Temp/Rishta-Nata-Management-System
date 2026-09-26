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

        /// <summary>The representative's "acting for" declaration (Gap 2).</summary>
        public string? ActingFor { get; set; }

        public string? Signature { get; set; }

        public DateTime? Date { get; set; }

        public string ReferenceNumber { get; set; } = string.Empty;

        public JamaatMember? JamaatMember { get; set; }

        // ===== Representative (Wakeel) the guardian appoints — Gap 2 =====

        /// <summary>
        /// Declared by the guardian on their own shared-link form. When true the
        /// representative's signature is required before AwaitingWitnesses completes.
        /// </summary>
        public bool AppointsRepresentative { get; set; }

        public string RepresentativeName { get; set; } = string.Empty;

        public string RepresentativeAddress { get; set; } = string.Empty;

        public string? RepresentativeSignature { get; set; }

        public DateTime? RepresentativeDate { get; set; }

    }
}
