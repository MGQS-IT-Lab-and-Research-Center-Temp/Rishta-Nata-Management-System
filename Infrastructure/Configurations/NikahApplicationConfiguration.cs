using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Persistence model for the new typed Nikah aggregate. This is intentionally
/// separate from the legacy MarriageApplicationForm tables during migration.
/// </summary>
public class NikahApplicationConfiguration : IEntityTypeConfiguration<NikahApplication>
{
    public void Configure(EntityTypeBuilder<NikahApplication> builder)
    {
        builder.ToTable("NikahApplications");
        builder.HasKey(application => application.Id);
        builder.Property(application => application.ReferenceNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(application => application.ReferenceNumber).IsUnique();
        builder.Property(application => application.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(application => application.AwaitingReviewStage).HasConversion<string>().HasMaxLength(50);
        builder.Property(application => application.Venue).HasMaxLength(200).IsRequired();

        builder.HasOne(application => application.Bride)
            .WithOne()
            .HasForeignKey<BrideDetails>("NikahApplicationId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(application => application.Bridegroom)
            .WithOne()
            .HasForeignKey<BridegroomDetails>("NikahApplicationId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(application => application.GuardianRepresentation)
            .WithOne(representation => representation.NikahApplication)
            .HasForeignKey<GuardianRepresentation>(representation => representation.NikahApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(application => application.Witnesses)
            .WithOne(witness => witness.NikahApplication)
            .HasForeignKey(witness => witness.NikahApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(application => application.Documents)
            .WithOne(document => document.NikahApplication)
            .HasForeignKey(document => document.NikahApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(application => application.WorkflowDecisions)
            .WithOne(decision => decision.NikahApplication)
            .HasForeignKey(decision => decision.NikahApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(application => application.CorrectionRequests)
            .WithOne(request => request.NikahApplication)
            .HasForeignKey(request => request.NikahApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(application => application.Certificates)
            .WithOne(certificate => certificate.NikahApplication)
            .HasForeignKey(certificate => certificate.NikahApplicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
