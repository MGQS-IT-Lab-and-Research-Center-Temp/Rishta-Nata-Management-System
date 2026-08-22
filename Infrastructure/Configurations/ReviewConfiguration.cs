using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Comment
            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(1000);

            // Marriage Application ID
            builder.Property(r => r.FormApplicationId)
                .IsRequired();

            // Status
            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(50);

            // Reviewed At
            builder.Property(r => r.ReviewedAt)
                .IsRequired();

            // Relationship with MarriageApplication
            builder.HasOne(r => r.FormApplication)
                .WithMany()
                .HasForeignKey(r => r.FormApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reviewer
            builder.Property(r => r.ReviewerId)
                .IsRequired();
        }
    }
}
