using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RishtaNata.Domain.Entities;

namespace RishtaNata.Infrastructure.Persistence.Configurations;

public class MarriageApplicationConfiguration
    : IEntityTypeConfiguration<MarriageApplication>
{
    public void Configure(EntityTypeBuilder<MarriageApplication> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ApplicationUserId)
            .IsRequired();

        builder.Property(x => x.NikahSerialNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.HasIndex(x => x.NikahSerialNumber)
            .IsUnique();
    }
}