using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel;

public class WitnessViewModel
{
    [Required]
    public Guid MarriageApplicationId { get; set; }

    [Display(Name = "Reference Number")]
    [Required]
    public string ReferenceNumber { get; set; } = string.Empty;

    public string BrideName { get; set; } = string.Empty;
    public string BridegroomName { get; set; } = string.Empty;

    [Display(Name = "Witness 1 is a member")]
    public bool WitnessOneIsMember { get; set; }

    [Display(Name = "Witness 1 Membership No")]
    public string WitnessOneMembershipNo { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Name")]
    public string WitnessOneName { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Address")]
    public string WitnessOneAddress { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Telephone")]
    public string WitnessOneTel { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Signature Date")]
    public string WitnessOneSignatureDate { get; set; } = string.Empty;

    [Display(Name = "Witness 2 is a member")]
    public bool WitnessTwoIsMember { get; set; }

    [Display(Name = "Witness 2 Membership No")]
    public string WitnessTwoMembershipNo { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Name")]
    public string WitnessTwoName { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Address")]
    public string WitnessTwoAddress { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Telephone")]
    public string WitnessTwoTel { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Signature Date")]
    public string WitnessTwoSignatureDate { get; set; } = string.Empty;
}
