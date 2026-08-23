using System;
using Domain.Abstractions;

namespace Domain.Entities
{
    public class BridegroomFormSection : AuditableEntity
    {


        public string BridegroomMembershipNo { get; set; } = string.Empty;
        public string BridegroomName { get; set; } = string.Empty;
        public DateTime BridegroomDateOfBirth { get; set; }
        public string BridegroomResidentOf { get; set; } = string.Empty;
        public string BridegroomGenotype { get; set; } = string.Empty;
        public string BridegroomBloodGroup { get; set; } = string.Empty;
        public decimal BridegroomDowerAmountPaidInCash { get; set; }
        public decimal BridegroomDowerAmountToBePaid { get; set; }
        public bool IsFirstNikah { get; set; }
        public bool IsSecondThirdOrFourthNikah { get; set; }
        public bool FormerWifeIsDead { get; set; }
        public bool HasDivorcedFormerWife { get; set; }
        public bool FormerWifeIsPresent { get; set; }
        public bool FormerWifeObtainedKhula { get; set; }
        public string BridegroomSignatureTel { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString();
    }
}

public enum NikahOrdinal
{
    First,
    SecondThirdOrFourth
}