using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Presentation.ViewModel;

public class WitnessViewModel
{
    public Guid Id { get; set; }

    public Guid MarriageApplicationFormId { get; set; }

    [Required(ErrorMessage = "Full name is required.")]
    [Display(Name = "Full Name")]
    public string? FullName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [Display(Name = "Email Address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Signature date is required.")]
    public string? SignatureDate { get; set; }

    // Set by the system when the witness is created
    public WitnessRole Role { get; set; }

    // Set by the system
    public int WitnessNumber { get; set; }

    // Used to identify the witness invitation
    public string InvitationToken { get; set; } = string.Empty;

    // Controlled by the system
    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }
}