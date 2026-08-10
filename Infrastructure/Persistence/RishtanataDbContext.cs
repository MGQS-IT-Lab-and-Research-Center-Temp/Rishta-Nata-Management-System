using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class RishtanataDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public RishtanataDbContext(DbContextOptions<RishtanataDbContext> options)
        : base(options) { }

    public DbSet<Certificate> Certificates => Set<Certificate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RishtanataDbContext).Assembly);
    }
}
