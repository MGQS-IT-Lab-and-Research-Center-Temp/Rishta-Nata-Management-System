using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AuditLogDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options)
            : base(options) { }
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AuditLogDbContext).Assembly);
        }
    }
}
