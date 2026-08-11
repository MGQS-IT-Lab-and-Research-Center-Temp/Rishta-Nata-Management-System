using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public string Description { get; set; } = default!;
}
