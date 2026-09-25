using System.ComponentModel.DataAnnotations;
using Domain.Enums;

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
    [Display(Name = "Marital Status")]
    public BrideMaritalStatus? MaritalStatus { get; set; }
    public string BrideDivorceEvidence { get; set; } = string.Empty;

    // Gap 8: the certificate itself; the text above is now an optional reference number.
    [Display(Name = "Khula certificate (PDF, JPG or PNG, max 5 MB)")]
    public IFormFile? BrideDivorceEvidenceFile { get; set; }

    public decimal ProposedDowerAmount { get; set; }
    public decimal DowerAmountReceivedInCash { get; set; }

    // ===== Groom-only =====
    public decimal DowerAmountPaidInCash { get; set; }
    public decimal DowerAmountToBePaid { get; set; }
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
    public bool FormerWifeIsDead { get; set; }
    public bool HasDivorcedFormerWife { get; set; }
    public string BridegroomDivorceEvidence { get; set; } = string.Empty;

    [Display(Name = "Talaq certificate (PDF, JPG or PNG, max 5 MB)")]
    public IFormFile? BridegroomDivorceEvidenceFile { get; set; }

    // Gap 8: the certificate already on file for this party, shown so a new
    // upload is clearly a replacement. Display only; never posted.
    public string? ExistingDivorceEvidenceFileName { get; set; }

    public bool FormerWifeIsPresent { get; set; }
    public bool FormerWifeObtainedKhula { get; set; }

    public bool CanAttendNikahInPerson { get; set; } = true;
    public string WakeelName { get; set; } = string.Empty;
    public string WakeelTel { get; set; } = string.Empty;
}
