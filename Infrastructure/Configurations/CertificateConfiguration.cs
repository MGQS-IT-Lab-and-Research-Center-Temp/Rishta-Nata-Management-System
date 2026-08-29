using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(c => c.Id);

        // ============================================================
        // One certificate per FormApplication — Certificate is the
        // DEPENDENT side (it holds the foreign key).
        //
        // The Certificates table was migrated with the FK column named
        // "MarriageApplicationId" (from the original entity model that
        // called the application "MarriageApplication"). The entity now
        // names it FormApplicationId, so map it to the existing column
        // to keep the schema unchanged.
        // ============================================================
        builder.HasOne(c => c.FormApplication)
            .WithOne(fa => fa.Certificate)
            .HasForeignKey<Certificate>(c => c.FormApplicationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.FormApplicationId)
            .HasColumnName("MarriageApplicationId");

        builder.HasIndex(c => c.FormApplicationId)
            .IsUnique();

        // Stale leftovers from an earlier model: Certificate used to carry
        // an int-named FK to MarriageApplicationForm, but that entity's key
        // is a Guid and no such column exists in the migrated schema.
        // Excluding them here prevents EF from inventing a broken one-to-one.
        builder.Ignore(c => c.MarriageApplicationForm);
        builder.Ignore(c => c.MarriageApplicationFormId);

        builder.Property(c => c.IssueDate)
            .IsRequired();

        builder.Property(c => c.CertificateFilePath)
            .IsRequired(false);
    }
}