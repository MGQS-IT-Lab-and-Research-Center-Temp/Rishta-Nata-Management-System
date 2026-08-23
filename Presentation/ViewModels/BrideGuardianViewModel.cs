using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel;

public class BrideGuardianViewModel
{
    [Required]
    public Guid MarriageApplicationId { get; set; }

    [Display(Name = "Reference Number")]
    [Required]
    public string ReferenceNumber { get; set; } = string.Empty;

    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BrideName { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BrideFatherName { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public DateTime BrideDateOfBirth { get; set; }
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BrideResidentOf { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BrideGenotype { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BrideBloodGroup { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BrideMaritalStatus { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public decimal BrideProposedDowerAmount { get; set; }
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public decimal BrideDowerAmountReceivedInCash { get; set; }

    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BridegroomName { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BridegroomFatherName { get; set; } = string.Empty;
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public DateTime BridegroomDateOfBirth { get; set; }
    // [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
    public string BridegroomResidentOf { get; set; } = string.Empty;

    [Display(Name = "Guardian / Waliyy Name")]
    [Required]
    public string GuardianName { get; set; } = string.Empty;

    [Display(Name = "Relationship to Bride")]
    [Required]
    public string GuardianRelationToBride { get; set; } = string.Empty;

    [Display(Name = "Guardian Address")]
    [Required]
    public string GuardianAddress { get; set; } = string.Empty;

    [Display(Name = "Telephone")]
    [Required]
    public string GuardianTel { get; set; } = string.Empty;

    [Display(Name = "Signature Date")]
    [Required]
    public string GuardianSignatureDate { get; set; } = string.Empty;
}