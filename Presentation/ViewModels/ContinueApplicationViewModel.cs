using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

public class ContinueApplicationViewModel
{
    public Guid Id { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    // "Bride" or "Groom" — which party the logged-in member is completing.
    public string Party { get; set; } = string.Empty;

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

    // ===== Bride-only =====
    public string MaritalStatus { get; set; } = string.Empty;
    public decimal ProposedDowerAmount { get; set; }
    public decimal DowerAmountReceivedInCash { get; set; }

    // ===== Groom-only =====
    public decimal DowerAmountPaidInCash { get; set; }
    public decimal DowerAmountToBePaid { get; set; }
    public bool IsFirstNikah { get; set; }
    public bool IsSecondThirdOrFourthNikah { get; set; }
    public bool FormerWifeIsDead { get; set; }
    public bool HasDivorcedFormerWife { get; set; }
    public bool FormerWifeIsPresent { get; set; }
    public bool FormerWifeObtainedKhula { get; set; }
}
