using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence;

public class RishtanataDbContext : DbContext
{
    public RishtanataDbContext(DbContextOptions<RishtanataDbContext> options) : base(options)
    {
    }
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<AqeeqahCertificate> AqeeqahCertificates => Set<AqeeqahCertificate>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<JamaatMember> JamaatMembers { get; set; }
    public DbSet<NikahApplication> NikahApplications => Set<NikahApplication>();
    public DbSet<NikahCertificate> NikahCertificates => Set<NikahCertificate>();
    public DbSet<SupportingDocument> SupportingDocuments => Set<SupportingDocument>();
    public DbSet<NikahWorkflowDecision> NikahWorkflowDecisions => Set<NikahWorkflowDecision>();
    public DbSet<NikahCorrectionRequest> NikahCorrectionRequests => Set<NikahCorrectionRequest>();
    public DbSet<Role> JamaatRoles => Set<Role>();
    public DbSet<JamaatMemberRole> JamaatMemberRoles => Set<JamaatMemberRole>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RishtanataDbContext).Assembly);
    }
}
