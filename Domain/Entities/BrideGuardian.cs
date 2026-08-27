
using Domain.Abstractions;

namespace Domain.Entities
{
    public class BrideGuardian : AuditableEntity
    {
        public Guid BrideGuardianId { get; set; }
        public Guid MarriageApplicationId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string GuardianName { get; set; } = string.Empty;
        public string GuardianRelationToBride { get; set; } = string.Empty;
        public string GuardianAddress { get; set; } = string.Empty;
        public string GuardianTel { get; set; } = string.Empty;
        public string GuardianSignatureDate { get; set; } = string.Empty;
        public ICollection<JamaatMember> Brides { get; set; } = new List<JamaatMember>();
    }
}