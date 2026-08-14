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
    public  DbSet<Review> MarriageCertificates => Set<Review>();

    public DbSet<MarriageApplication> MarriageApplications => Set<MarriageApplication>();
    public DbSet<MarriageApplicationForm> MarriageApplicationForms => Set<MarriageApplicationForm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RishtanataDbContext).Assembly);
    }
}