using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class MarriageFormRejectionConfiguration : IEntityTypeConfiguration<MarriageFormRejection>
    {
        public void Configure(EntityTypeBuilder<MarriageFormRejection> builder)
        {
            // Marriage Application Form ID
            builder.Property(r => r.MarriageApplicationFormId)
                .IsRequired();

            // Rejected At Stage
            builder.Property(r => r.RejectedAtStage)
                .IsRequired();

            // Reverted To Stage
            builder.Property(r => r.RevertedToStage)
                .IsRequired();

            // Reason
            builder.Property(r => r.Reason)
                .IsRequired()
                .HasMaxLength(1000);

            // Relationship with MarriageApplicationForm (one-to-many)
            builder.HasOne(r => r.MarriageApplicationForm)
                .WithMany(f => f.Rejections)
                .HasForeignKey(r => r.MarriageApplicationFormId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}