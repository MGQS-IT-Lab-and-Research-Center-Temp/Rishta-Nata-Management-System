using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReviewerId)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Comment)
            .HasMaxLength(1000);

        builder.Property(r => r.ReviewedAt)
            .IsRequired();

        builder.HasOne(r => r.MarriageApplicationForm)
            .WithMany(f => f.Reviews)
            .HasForeignKey(r => r.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.MarriageApplicationFormId);
    }
}