using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class RishtanataDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public RishtanataDbContext(DbContextOptions<RishtanataDbContext> options)
        : base(options) { }
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RishtanataDbContext).Assembly);
    }
}
