using System;
using Domain.Abstractions;
using System.Collections.Generic;
namespace Domain.Entities
{
    public class MarriageApplicationForm : AuditableEntity
    {
        // ===== Application =====
        public Guid MarriageApplicationId { get; set; }

        public FormApplication MarriageApplication { get; set; } = null!;

        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime ProposedNikahDate { get; set; }
        public string Venue { get; set; } = string.Empty;

        // ===== Bride =====
        public string BrideMembershipNo { get; set; } = string.Empty;
        public string BrideName { get; set; } = string.Empty;
        public DateTime BrideDateOfBirth { get; set; }
        public string BrideResidentOf { get; set; } = string.Empty;
        public string BrideGenotype { get; set; } = string.Empty;
        public string BrideBloodGroup { get; set; } = string.Empty;
        public string BrideMaritalStatus { get; set; } = string.Empty;

        public decimal BrideProposedDowerAmount { get; set; }
        public decimal BrideDowerAmountReceivedInCash { get; set; }

        public string BrideSignatureTel { get; set; } = string.Empty;

        // ===== Bridegroom =====
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

        // ===== Bride's Parent =====
        public string BrideFatherName { get; set; } = string.Empty;

        // ===== Groom's Parent =====
        public string BridegroomFatherName { get; set; } = string.Empty;

        // ===== Guardian (Bride's Waliyy) =====
        public string GuardianName { get; set; } = string.Empty;
        public string GuardianRelationToBride { get; set; } = string.Empty;
        public string GuardianAddress { get; set; } = string.Empty;
        public string GuardianTel { get; set; } = string.Empty;
        public string GuardianSignatureDate { get; set; } = string.Empty;

        // ===== Representative (Wakeel) =====
        public string RepresentativeName { get; set; } = string.Empty;
        public string RepresentativeAddress { get; set; } = string.Empty;
        public string RepresentativeActingFor { get; set; } = string.Empty;
        public string RepresentativeSignatureDate { get; set; } = string.Empty;

        // ===== Verification & Approval =====
        public string OfficiatingImamName { get; set; } = string.Empty;
        public string OfficiatingImamAddressJamaat { get; set; } = string.Empty;
        public string OfficiatingImamSignatureDate { get; set; } = string.Empty;

        public string JamaatPresidentName { get; set; } = string.Empty;
        public string JamaatPresidentSignatureDate { get; set; } = string.Empty;

        public string NationalRishtanataSecretaryName { get; set; } = string.Empty;
        public string NationalRishtanataSecretarySignatureDate { get; set; } = string.Empty;

        public DateTime? ApprovedDateOfNikah { get; set; }

        public string NationalAmirOrMissionarySignatureDate { get; set; } = string.Empty;

        public ICollection<Witness> Witnesses { get; set; }= new List<Witness>();
        // ===== Rejection Audit Trail =====
        public ICollection<MarriageFormRejection> Rejections { get; set; } = new List<MarriageFormRejection>();
    }
}