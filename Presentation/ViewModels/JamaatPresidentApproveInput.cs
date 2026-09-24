using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

/// <summary>
/// What the Jama'at President enters when signing (Approve). Holding needs
/// none of it. Gap 4: the Local Rishtanata Secretary block for the side this
/// president signs for. MarriageFormWorkflowService re-validates it, so this
/// is the friendly first pass, not the only check.
/// </summary>
public class JamaatPresidentApproveInput
{
    [Required(ErrorMessage = "Enter the Local Rishtanata Secretary's name.")]
    [StringLength(200)]
    [Display(Name = "Local Rishtanata Secretary's name")]
    public string LocalRishtanataSecretaryName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter the Local Rishtanata Secretary's telephone.")]
    [StringLength(30)]
    [Display(Name = "Telephone")]
    public string LocalRishtanataSecretaryTel { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter the Local Rishtanata Secretary's signature date.")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Enter the signature date as yyyy-MM-dd.")]
    [Display(Name = "Signature date")]
    public string LocalRishtanataSecretarySignatureDate { get; set; } = string.Empty;
}
