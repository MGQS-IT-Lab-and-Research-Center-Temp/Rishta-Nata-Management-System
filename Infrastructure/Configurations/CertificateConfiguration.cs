using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(c => c.Id);

        // One certificate per marriage application.
        builder.HasOne(c => c.MarriageApplication)
            .WithOne(ma => ma.Certificate)
            .HasForeignKey<Certificate>(c => c.MarriageApplicationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.MarriageApplicationId)
            .IsUnique();

        // Certificate serial number.
        builder.Property(c => c.SerialNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.SerialNumber)
            .IsUnique();

        // Date the certificate was issued.
        builder.Property(c => c.IssueDate)
            .IsRequired();

        // User who issued the certificate.
        builder.Property(c => c.IssuedByUserId)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.IssuedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Generated certificate file path.
        builder.Property(c => c.CertificateFilePath)
            .IsRequired(false);
    }
}