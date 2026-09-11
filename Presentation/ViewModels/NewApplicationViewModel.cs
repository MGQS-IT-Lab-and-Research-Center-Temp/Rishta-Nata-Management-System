using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Presentation.ViewModels;

public class NewApplicationViewModel
{
    // "Groom" or "Bride" — pre-selected from the logged-in member's Sex but
    // overridable so the applicant can correct an incorrect auto-detection.
    public string StartingParty { get; set; } = "Groom";

    [Required(ErrorMessage = "A proposed Nikah date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Proposed Nikah Date")]
    public DateTime ProposedNikahDate { get; set; } = DateTime.Today.AddDays(30);

    [Required(ErrorMessage = "A venue is required.")]
    [Display(Name = "Venue / Jama'at")]
    public string Venue { get; set; } = string.Empty;

    public ApplicantPartyInfo Bride { get; set; } = new();
    public ApplicantPartyInfo Bridegroom { get; set; } = new();

    // ===== Bride-only =====
    [Display(Name = "Marital Status")]
    public string BrideMaritalStatus { get; set; } = string.Empty;

    [Display(Name = "Divorce Evidence")]
    public string BrideDivorceEvidence { get; set; } = string.Empty;

    [Display(Name = "Proposed Dower Amount")]
    public decimal BrideProposedDowerAmount { get; set; }

    [Display(Name = "Dower Amount Received In Cash")]
    public decimal BrideDowerAmountReceivedInCash { get; set; }

    // ===== Groom-only =====
    [Display(Name = "Dower Amount Paid In Cash")]
    public decimal BridegroomDowerAmountPaidInCash { get; set; }

    [Display(Name = "Dower Amount To Be Paid")]
    public decimal BridegroomDowerAmountToBePaid { get; set; }
    public bool IsFirstNikah { get; set; }

    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
    public bool? FormerWifeIsDead { get; set; }
    public bool? HasDivorcedFormerWife { get; set; }
    public string BridegroomDivorceEvidence { get; set; } = string.Empty;
    public bool? FormerWifeIsPresent { get; set; }
    public bool? FormerWifeObtainedKhula { get; set; }
}

public class ApplicantPartyInfo
{
    [Required(ErrorMessage = "Membership number is required.")]
    [Display(Name = "Membership Number")]
    public string MembershipNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Residential address is required.")]
    [Display(Name = "Residential Address")]
    public string ResidentOf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

    public string Genotype { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
}
