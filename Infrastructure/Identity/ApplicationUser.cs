using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public bool IsActive { get; set; }
    public bool MustChangePassword { get; set; }

    public class ApplicationUser : IdentityUser
    {
    }
}
