using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DivorceEvidenceDocumentConfiguration
    : IEntityTypeConfiguration<DivorceEvidenceDocument>
{
    public void Configure(EntityTypeBuilder<DivorceEvidenceDocument> builder)
    {
        builder.ToTable("DivorceEvidenceDocuments");

        builder.HasKey(d => d.Id);

        // Stored by name (Bride/Bridegroom), like BrideMaritalStatus (Gap 6).
        builder.Property(d => d.Party)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Every string has a max length so MySQL creates varchar, not longtext.
        builder.Property(d => d.OriginalFileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.StoredFileName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.ContentType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.UploadedBy)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(d => d.StoredFileName)
            .IsUnique();

        // One certificate per party per form; a new upload replaces the row.
        builder.HasIndex(d => new { d.MarriageApplicationFormId, d.Party })
            .IsUnique();

        builder.HasOne(d => d.MarriageApplicationForm)
            .WithMany(f => f.DivorceEvidenceDocuments)
            .HasForeignKey(d => d.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
