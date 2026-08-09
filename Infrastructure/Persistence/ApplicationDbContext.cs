using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;

namespace Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}