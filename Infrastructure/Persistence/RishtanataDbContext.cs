using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class RishtanataDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public RishtanataDbContext(DbContextOptions<RishtanataDbContext> options)
        : base(options) { }
    public DbSet<JamaatMember> JamaatMembers { get; set; }
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<AqeeqahCertificate> AqeeqahCertificates => Set<AqeeqahCertificate>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<FormApplication> FormApplications => Set<FormApplication>();
    public DbSet<MarriageApplicationForm> MarriageApplicationForms => Set<MarriageApplicationForm>();
    public DbSet<BrideGuardian> BrideGuardians => Set<BrideGuardian>();
    public DbSet<Role> JamaatRoles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RishtanataDbContext).Assembly);
    }
}
