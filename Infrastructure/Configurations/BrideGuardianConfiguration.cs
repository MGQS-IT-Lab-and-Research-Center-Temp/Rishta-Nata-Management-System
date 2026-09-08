using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configurations;

public class BrideGuardianConfiguration : IEntityTypeConfiguration<BrideGuardian>
{
    public void Configure(EntityTypeBuilder<BrideGuardian> builder)
    {
        builder.HasKey(x => x.BrideGuardianId);
        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(x => x.GuardianName)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(x => x.GuardianRelationToBride)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.GuardianAddress)
            .HasMaxLength(300)
            .IsRequired();
        builder.Property(x => x.GuardianTel)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(x => x.GuardianSignatureDate)
            .HasMaxLength(50)
            .IsRequired();
        // A JamaatMember may or may not have a guardian (e.g. brides
        // typically do; jamaat presidents, bridegrooms, etc. do not) —
        // this relationship is optional at the database level.
        builder.HasMany(x => x.Brides)
            .WithOne(x => x.BrideGuardian)
            .HasForeignKey(x => x.BrideGuardianId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}