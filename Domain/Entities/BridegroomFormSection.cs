using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Abstractions;

namespace Domain.Entities
{
    public class BridegroomFormSection : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Guid Id { get; set; }

        // Bio-data
        public string BridegroomName { get; set; } = string.Empty;
        public DateTime BridegroomDateOfBirth { get; set; }
        public string BridegroomResidentOf { get; set; } = string.Empty;
        public string BridegroomGenotype { get; set; } = string.Empty;
        public string BridegroomBloodGroup { get; set; } = string.Empty;
        public string BridegroomPhoneNumber { get; set; } = string.Empty;

        // Dower
        public decimal DowerAmountPaidInCash { get; set; }
        public decimal DowerAmountToBePaid { get; set; }

        // Nikah history
        public NikahOrdinal NikahOrdinal { get; set; }

        // Note: when NikahOrdinal != First, exactly one of the first three flags should be true
        public bool FormerWifeIsDead { get; set; }
        public bool HasDivorcedFormerWife { get; set; }
        public bool FormerWifeObtainedKhula { get; set; }
        public bool FormerWifeIsPresent { get; set; }
    }

    // 👇 Enum should be declared at the same namespace level, not inside the class
    public enum NikahOrdinal
    {
        First,
        SecondThirdOrFourth
    }
}
