using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public string Description { get; set; } = default!;
    public class ApplicationRole : IdentityRole
    {
    }
}
