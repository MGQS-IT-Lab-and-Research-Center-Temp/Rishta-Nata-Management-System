using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class MarriageApplicationConfiguration : IEntityTypeConfiguration<MarriageApplication>
    {
        public void Configure(EntityTypeBuilder<MarriageApplication> builder)
        { 
            builder.HasKey(ma => ma.Id);

            builder.Property(ma => ma.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ma => ma.UserId)
                .IsRequired();

            builder.HasOne(ma => ma.Certificate)
                .WithOne()
                .HasForeignKey<MarriageApplication>(ma => ma.Certificate)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(ma => ma.SerialNumber)
                .IsUnique();

            builder.Property(ma => ma.SerialNumber)
                .HasMaxLength(50)
                .IsRequired(false);
        }
    }
}
