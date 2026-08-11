using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser: IdentityUser<Guid>
{
    public bool IsActive { get; set; }
    public bool MustChangePassword { get; set; }
}
