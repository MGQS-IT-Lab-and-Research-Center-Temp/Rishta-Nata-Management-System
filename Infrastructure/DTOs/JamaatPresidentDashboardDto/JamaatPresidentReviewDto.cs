using Domain.Enums;

namespace Infrastructure.DTOs.JamaatPresidentDashboardDto;

public class JamaatPresidentReviewDto
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime SubmittedDate { get; set; }

    /// <summary>The form's current review-chain stage, for the revert modal.</summary>
    public ApplicationStage? CurrentStage { get; set; }

    /// <summary>The form's fine-grained form stage, read-only. Lets the controller
    /// dispatch Approve/Reject to the matching workflow submit without writing any
    /// stage itself (workflow remains the single source of truth).</summary>
    public MarriageFormStage? CurrentFormStage { get; set; }

    /// <summary>The bride's president's phone, surfaced for the workflow submission
    /// payload (the flat form carries no Tel column).</summary>
    public string JamaatPresidentTel { get; set; } = string.Empty;

    /// <summary>The groom's president's phone (different-Jama'at path only).</summary>
    public string GroomJamaatPresidentTel { get; set; } = string.Empty;

    /// <summary>The groom's president's name (different-Jama'at path only).</summary>
    public string GroomJamaatPresidentName { get; set; } = string.Empty;

    /// <summary>The groom's president's signature date (different-Jama'at path only).</summary>
    public string GroomJamaatPresidentSignatureDate { get; set; } = string.Empty;
    public DateTime ProposedNikahDate { get; set; }
    public string Venue { get; set; } = string.Empty;

    public string BrideMembershipNo { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public DateTime BrideDateOfBirth { get; set; }
    public string BrideResidentOf { get; set; } = string.Empty;
    public string BrideGenotype { get; set; } = string.Empty;
    public string BrideBloodGroup { get; set; } = string.Empty;
    public BrideMaritalStatus? BrideMaritalStatus { get; set; }
    public decimal BrideProposedDowerAmount { get; set; }
    public decimal BrideDowerAmountReceivedInCash { get; set; }
    public string BrideSignatureTel { get; set; } = string.Empty;

    public string BridegroomMembershipNo { get; set; } = string.Empty;
    public string BridegroomName { get; set; } = string.Empty;
    public DateTime BridegroomDateOfBirth { get; set; }
    public string BridegroomResidentOf { get; set; } = string.Empty;
    public string BridegroomGenotype { get; set; } = string.Empty;
    public string BridegroomBloodGroup { get; set; } = string.Empty;
    public decimal BridegroomDowerAmountPaidInCash { get; set; }
    public decimal BridegroomDowerAmountToBePaid { get; set; }
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
    public bool FormerWifeIsDead { get; set; }
    public bool HasDivorcedFormerWife { get; set; }
    public bool FormerWifeIsPresent { get; set; }
    public bool FormerWifeObtainedKhula { get; set; }
    public string BridegroomSignatureTel { get; set; } = string.Empty;

    public string BrideFatherName { get; set; } = string.Empty;
    public string BridegroomFatherName { get; set; } = string.Empty;

    public string GuardianName { get; set; } = string.Empty;
    public string GuardianRelationToBride { get; set; } = string.Empty;
    public string GuardianAddress { get; set; } = string.Empty;
    public string GuardianTel { get; set; } = string.Empty;
    public string GuardianSignatureDate { get; set; } = string.Empty;

    public string RepresentativeName { get; set; } = string.Empty;
    public string RepresentativeAddress { get; set; } = string.Empty;
    public string RepresentativeActingFor { get; set; } = string.Empty;
    public string RepresentativeSignatureDate { get; set; } = string.Empty;

    public string GroomWakeelName { get; set; } = string.Empty;
    public string GroomWakeelFatherName { get; set; } = string.Empty;
    public string GroomWakeelTel { get; set; } = string.Empty;
    public string GroomWakeelSignatureDate { get; set; } = string.Empty;

    public string WitnessOneName { get; set; } = string.Empty;
    public string WitnessOneAddress { get; set; } = string.Empty;
    public string WitnessOneTel { get; set; } = string.Empty;
    public string WitnessOneSignatureDate { get; set; } = string.Empty;

    public string WitnessTwoName { get; set; } = string.Empty;
    public string WitnessTwoAddress { get; set; } = string.Empty;
    public string WitnessTwoTel { get; set; } = string.Empty;
    public string WitnessTwoSignatureDate { get; set; } = string.Empty;

    public string OfficiatingImamName { get; set; } = string.Empty;
    public string OfficiatingImamAddressJamaat { get; set; } = string.Empty;
    public string OfficiatingImamSignatureDate { get; set; } = string.Empty;

    public string JamaatPresidentName { get; set; } = string.Empty;
    public string JamaatPresidentSignatureDate { get; set; } = string.Empty;

    /// <summary>Recorded Local Rishtanata Secretary entries per side (Gap 4). The
    /// groom side falls back to the bride side on the same-Jama'at path.</summary>
    public string BrideLocalRishtanataSecretaryName { get; set; } = string.Empty;
    public string BrideLocalRishtanataSecretarySignatureDate { get; set; } = string.Empty;
    public string GroomLocalRishtanataSecretaryName { get; set; } = string.Empty;
    public string GroomLocalRishtanataSecretarySignatureDate { get; set; } = string.Empty;

    public string NationalRishtanataSecretaryName { get; set; } = string.Empty;
    public string NationalRishtanataSecretarySignatureDate { get; set; } = string.Empty;

    public DateTime? ApprovedDateOfNikah { get; set; }

    /// <summary>True when the bride and groom belong to the same Jama'at
    /// (the president signs once for both partners).</summary>
    public bool PartnersShareJamaat { get; set; }

    public string NationalAmirOrMissionarySignatureDate { get; set; } = string.Empty;
}