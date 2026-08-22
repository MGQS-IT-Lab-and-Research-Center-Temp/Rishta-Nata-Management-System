using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Chanda no is required.")]
    public string ChandaNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}