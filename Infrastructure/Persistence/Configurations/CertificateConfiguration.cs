using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.HasKey(c => c.Id);

            // Enforces one certificate per application at the DB level.
            // TODO: once MarriageApplication exists, replace this with a proper
            // HasOne(c => c.MarriageApplication).WithOne().HasForeignKey<Certificate>(c => c.MarriageApplicationId)
            // and keep IsRequired() + the unique index below.
            builder.Property(c => c.MarriageApplicationId)
                .IsRequired();

            builder.HasIndex(c => c.MarriageApplicationId)
                .IsUnique();

            builder.Property(c => c.IssueDate)
                .IsRequired();

            builder.Property(c => c.IssuedByUserId)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(c => c.IssuedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.CertificateFilePath)
                .IsRequired(false);
        }
    }
}