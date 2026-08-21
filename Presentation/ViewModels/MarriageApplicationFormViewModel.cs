using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel;

public class MarriageApplicationFormViewModel
{
    // Application
    public Guid MarriageApplicationId { get; set; }

    [Display(Name = "Reference Number")]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Display(Name = "Proposed Nikah Date")]
    [DataType(DataType.Date)]
    public DateTime ProposedNikahDate { get; set; }

    public string Venue { get; set; } = string.Empty;

    // Bride
    public string BrideMembershipNo { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime BrideDateOfBirth { get; set; }

    public string BrideResidentOf { get; set; } = string.Empty;
    public string BrideGenotype { get; set; } = string.Empty;
    public string BrideBloodGroup { get; set; } = string.Empty;
    public string BrideMaritalStatus { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    public decimal BrideProposedDowerAmount { get; set; }

    [DataType(DataType.Currency)]
    public decimal BrideDowerAmountReceivedInCash { get; set; }

    public string BrideSignatureTel { get; set; } = string.Empty;

    // Bridegroom
    public string BridegroomMembershipNo { get; set; } = string.Empty;
    public string BridegroomName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime BridegroomDateOfBirth { get; set; }

    public string BridegroomResidentOf { get; set; } = string.Empty;
    public string BridegroomGenotype { get; set; } = string.Empty;
    public string BridegroomBloodGroup { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    public decimal BridegroomDowerAmountPaidInCash { get; set; }

    [DataType(DataType.Currency)]
    public decimal BridegroomDowerAmountToBePaid { get; set; }

    public bool IsFirstNikah { get; set; }
    public bool IsSecondThirdOrFourthNikah { get; set; }

    public bool FormerWifeIsDead { get; set; }
    public bool HasDivorcedFormerWife { get; set; }
    public bool FormerWifeIsPresent { get; set; }
    public bool FormerWifeObtainedKhula { get; set; }

    public string BridegroomSignatureTel { get; set; } = string.Empty;

    // Parents
    public string BrideFatherName { get; set; } = string.Empty;
    public string BridegroomFatherName { get; set; } = string.Empty;

    // Guardian / Waliyy
    public string GuardianName { get; set; } = string.Empty;
    public string GuardianRelationToBride { get; set; } = string.Empty;
    public string GuardianAddress { get; set; } = string.Empty;
    public string GuardianTel { get; set; } = string.Empty;
    public string GuardianSignatureDate { get; set; } = string.Empty;

    // Representative / Wakeel
    public string RepresentativeName { get; set; } = string.Empty;
    public string RepresentativeAddress { get; set; } = string.Empty;
    public string RepresentativeActingFor { get; set; } = string.Empty;
    public string RepresentativeSignatureDate { get; set; } = string.Empty;

    // Witnesses
    public List<WitnessViewModel> Witnesses { get; set; } = new();

    // Verification & Approval
    public string OfficiatingImamName { get; set; } = string.Empty;
    public string OfficiatingImamAddressJamaat { get; set; } = string.Empty;
    public string OfficiatingImamSignatureDate { get; set; } = string.Empty;

    public string JamaatPresidentName { get; set; } = string.Empty;
    public string JamaatPresidentSignatureDate { get; set; } = string.Empty;

    public string NationalRishtanataSecretaryName { get; set; } = string.Empty;
    public string NationalRishtanataSecretarySignatureDate { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? ApprovedDateOfNikah { get; set; }

    public string NationalAmirOrMissionarySignatureDate { get; set; } = string.Empty;
}