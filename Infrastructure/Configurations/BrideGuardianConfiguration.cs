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

        builder.HasMany(x => x.Brides)
            .WithOne(x => x.BrideGuardian)
            .HasForeignKey(x => x.BrideGuardianId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}