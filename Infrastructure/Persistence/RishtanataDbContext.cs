using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence;

public class RishtanataDbContext : DbContext
{
    public RishtanataDbContext(DbContextOptions<RishtanataDbContext> options) : base(options)
    {
    }
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<AqeeqahCertificate> AqeeqahCertificates => Set<AqeeqahCertificate>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<JamaatMember> JamaatMembers { get; set; }
    public DbSet<FormApplication> FormApplications => Set<FormApplication>();
    public DbSet<MarriageApplicationForm> MarriageApplicationForms => Set<MarriageApplicationForm>();
    public DbSet<BridegroomFormSection> BridegroomFormSections => Set<BridegroomFormSection>();
    public DbSet<MarriageFormRejection> MarriageFormRejections => Set<MarriageFormRejection>();
    public DbSet<Role> JamaatRoles => Set<Role>();
    public DbSet<JamaatMemberRole> JamaatMemberRoles => Set<JamaatMemberRole>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BridegroomFormSection>().ToTable("BrideGrooms");

        // Resolves an ambiguous 1:1 relationship: both Certificate and
        // FormApplication declare a FK to each other. A FormApplication is
        // created first (someone applies); a Certificate is issued
        // afterward, referencing the application it came from — so
        // Certificate is the dependent side.
        modelBuilder.Entity<Certificate>()
            .HasOne(c => c.FormApplication)
            .WithOne(f => f.Certificate)
            .HasForeignKey<Certificate>(c => c.FormApplicationId);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RishtanataDbContext).Assembly);
    }
}