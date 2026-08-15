using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.UserId).IsRequired();
            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.RecordId).IsRequired();
            builder.Property(a => a.Timestamp).IsRequired();
            builder.Property(a => a.ChangeDetails)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => new { a.EntityName, a.RecordId });
            builder.HasIndex(a => a.Timestamp);
        }
    }
}