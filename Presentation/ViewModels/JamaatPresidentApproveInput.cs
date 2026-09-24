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

    // ===== President's attestations (Gap 7) =====
    // Which partner blocks are required depends on the side being signed, so
    // they're checked by ValidateForSigning, not [Required].

    [Display(Name = "Born Ahmadi?")]
    public bool? BrideIsBornAhmadi { get; set; }

    [Range(0, 120, ErrorMessage = "Enter a number of years between 0 and 120.")]
    [Display(Name = "Years as an Ahmadi")]
    public int? BrideYearsAsAhmadi { get; set; }

    [StringLength(200)]
    [Display(Name = "Marriage reason")]
    public string BrideMarriageReason { get; set; } = string.Empty;

    [Display(Name = "Born Ahmadi?")]
    public bool? GroomIsBornAhmadi { get; set; }

    [Range(0, 120, ErrorMessage = "Enter a number of years between 0 and 120.")]
    [Display(Name = "Years as an Ahmadi")]
    public int? GroomYearsAsAhmadi { get; set; }

    [StringLength(200)]
    [Display(Name = "Marriage reason")]
    public string GroomMarriageReason { get; set; } = string.Empty;

    [Display(Name = "The bride's guardian (Wali) is bona fide")]
    public bool GuardianIsBonafide { get; set; }

    [Display(Name = "The bride signed this form of her own free will")]
    public bool BrideSignedFreely { get; set; }

    /// <summary>
    /// Friendly first pass over the attestations for the side being signed.
    /// MarriageFormWorkflowService re-validates. Field names are relative to
    /// this model; the controller prefixes them with "Approve.".
    /// </summary>
    public IEnumerable<(string Field, string Message)> ValidateForSigning(bool attestsBride, bool attestsGroom)
    {
        if (attestsBride)
        {
            foreach (var error in ValidatePartner(
                         BrideIsBornAhmadi, BrideYearsAsAhmadi,
                         nameof(BrideIsBornAhmadi), nameof(BrideYearsAsAhmadi), "bride"))
            {
                yield return error;
            }

            if (!GuardianIsBonafide)
            {
                yield return (nameof(GuardianIsBonafide), "Confirm that the guardian is bona fide.");
            }

            if (!BrideSignedFreely)
            {
                yield return (nameof(BrideSignedFreely), "Confirm that the bride signed freely.");
            }
        }

        if (attestsGroom)
        {
            foreach (var error in ValidatePartner(
                         GroomIsBornAhmadi, GroomYearsAsAhmadi,
                         nameof(GroomIsBornAhmadi), nameof(GroomYearsAsAhmadi), "bridegroom"))
            {
                yield return error;
            }
        }
    }

    private static IEnumerable<(string Field, string Message)> ValidatePartner(
        bool? isBornAhmadi, int? yearsAsAhmadi, string bornField, string yearsField, string partner)
    {
        if (isBornAhmadi is null)
        {
            yield return (bornField, $"Record whether the {partner} is a born Ahmadi.");
        }
        else if (isBornAhmadi == false && yearsAsAhmadi is null)
        {
            yield return (yearsField, $"Enter how many years the {partner} has been an Ahmadi.");
        }
    }
}
