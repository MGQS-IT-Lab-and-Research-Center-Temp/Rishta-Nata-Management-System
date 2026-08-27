using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

public class LoginViewModel
{
    // Login with Email or Chanda Number?
    /* 
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
    */

    [Required(ErrorMessage = "Chanda Number is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid chanda number")]

    public string ChandaNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}