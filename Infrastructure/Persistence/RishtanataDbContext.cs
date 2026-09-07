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
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<JamaatMember> JamaatMembers { get; set; }
    public DbSet<FormApplication> FormApplications => Set<FormApplication>();
    public DbSet<MarriageApplicationForm> MarriageApplicationForms => Set<MarriageApplicationForm>();
    public DbSet<BridegroomFormSection> BridegroomFormSections => Set<BridegroomFormSection>();
    public DbSet<BrideFormSection> BrideFormSections => Set<BrideFormSection>();
    public DbSet<MarriageFormRejection> MarriageFormRejections => Set<MarriageFormRejection>();
    public DbSet<SectionAccessToken> SectionAccessTokens => Set<SectionAccessToken>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BridegroomFormSection>().ToTable("BrideGrooms");

        // Section tables carry their own tenant identity (the owning
        // application's ReferenceNumber) for cross-domain identification.
        modelBuilder.Entity<BrideFormSection>(e =>
        {
            e.Property(x => x.ReferenceNumber).HasMaxLength(50);
            e.Property(x => x.BrideMembershipNo).HasMaxLength(50);
            e.Property(x => x.BrideName).HasMaxLength(200);
            e.Property(x => x.BrideResidentOf).HasMaxLength(300);
            e.Property(x => x.BrideGenotype).HasMaxLength(10);
            e.Property(x => x.BrideBloodGroup).HasMaxLength(10);
            e.Property(x => x.BrideMaritalStatus).HasMaxLength(50);
            e.Property(x => x.BrideProposedDowerAmount).HasColumnType("decimal(18,2)");
            e.Property(x => x.BrideDowerAmountReceivedInCash).HasColumnType("decimal(18,2)");
            e.Property(x => x.BrideSignatureTel).HasMaxLength(30);
        });

        modelBuilder.Entity<BridegroomFormSection>(e =>
            e.Property(x => x.ReferenceNumber).HasMaxLength(50));

        modelBuilder.Entity<GuardianOrWakeelSection>(e =>
            e.Property(x => x.ReferenceNumber).HasMaxLength(50));

        modelBuilder.Entity<WitnessSignatureSection>(e =>
            e.Property(x => x.ReferenceNumber).HasMaxLength(50));

        // One revocable share-link token per form+section. Regeneration
        // overwrites TokenHash in place, so at most one row ever exists per
        // (MarriageApplicationFormId, SectionType).
        modelBuilder.Entity<SectionAccessToken>(e =>
        {
            e.HasIndex(x => new { x.MarriageApplicationFormId, x.SectionType })
                .IsUnique();

            e.Property(x => x.TokenHash).HasMaxLength(64);
            e.Property(x => x.CreatedByMembershipNo).HasMaxLength(50);

            e.HasOne(x => x.MarriageApplicationForm)
                .WithMany(x => x.SectionAccessTokens)
                .HasForeignKey(x => x.MarriageApplicationFormId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Resolves an ambiguous 1:1 relationship: both Certificate and
        // FormApplication declare a FK to each other. A FormApplication is
        // created first (someone applies); a Certificate is issued
        // afterward, referencing the application it came from — so
        // Certificate is the dependent side.
        modelBuilder.Entity<Certificate>()
            .HasOne(c => c.FormApplication)
            .WithOne(f => f.Certificate)
            .HasForeignKey<Certificate>(c => c.FormApplicationId);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RishtanataDbContext).Assembly);
    }
}