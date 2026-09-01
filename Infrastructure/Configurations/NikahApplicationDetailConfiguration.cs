using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class NikahApplicationDetailConfiguration : IEntityTypeConfiguration<BrideDetails>,
    IEntityTypeConfiguration<BridegroomDetails>,
    IEntityTypeConfiguration<GuardianRepresentation>,
    IEntityTypeConfiguration<NikahWitnessAttestation>,
    IEntityTypeConfiguration<SupportingDocument>,
    IEntityTypeConfiguration<NikahWorkflowDecision>,
    IEntityTypeConfiguration<NikahCorrectionRequest>,
    IEntityTypeConfiguration<NikahCorrectionField>,
    IEntityTypeConfiguration<NikahCertificate>
{
    public void Configure(EntityTypeBuilder<BrideDetails> builder)
    {
        builder.ToTable("NikahBrideDetails");
        builder.Property("NikahApplicationId").IsRequired();
        builder.HasIndex("NikahApplicationId").IsUnique();
        builder.Property(details => details.MembershipNo).HasMaxLength(50).IsRequired();
        builder.Property(details => details.FullName).HasMaxLength(200).IsRequired();
        builder.Property(details => details.ProposedDowerAmount).HasColumnType("decimal(18,2)");
        builder.Property(details => details.DowerAmountReceivedInCash).HasColumnType("decimal(18,2)");
    }

    public void Configure(EntityTypeBuilder<BridegroomDetails> builder)
    {
        builder.ToTable("NikahBridegroomDetails");
        builder.Property("NikahApplicationId").IsRequired();
        builder.HasIndex("NikahApplicationId").IsUnique();
        builder.Property(details => details.MembershipNo).HasMaxLength(50).IsRequired();
        builder.Property(details => details.FullName).HasMaxLength(200).IsRequired();
        builder.Property(details => details.DowerAmountPaidInCash).HasColumnType("decimal(18,2)");
        builder.Property(details => details.DowerAmountToBePaid).HasColumnType("decimal(18,2)");
    }

    public void Configure(EntityTypeBuilder<GuardianRepresentation> builder)
    {
        builder.ToTable("NikahGuardianRepresentations");
        builder.HasIndex(representation => representation.NikahApplicationId).IsUnique();
        builder.Property(representation => representation.AttendanceOption).HasConversion<string>().HasMaxLength(50);
        builder.Property(representation => representation.GuardianMembershipNo).HasMaxLength(50).IsRequired();
        builder.Property(representation => representation.GuardianName).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<NikahWitnessAttestation> builder)
    {
        builder.ToTable("NikahWitnessAttestations");
        builder.HasIndex(witness => new { witness.NikahApplicationId, witness.Role }).IsUnique();
        builder.Property(witness => witness.Role).HasConversion<string>().HasMaxLength(50);
        builder.Property(witness => witness.InvitationTokenHash).HasMaxLength(256).IsRequired();
    }

    public void Configure(EntityTypeBuilder<SupportingDocument> builder)
    {
        builder.ToTable("NikahSupportingDocuments");
        builder.Property(document => document.Type).HasConversion<string>().HasMaxLength(50);
        builder.Property(document => document.OriginalFileName).HasMaxLength(255).IsRequired();
        builder.Property(document => document.StorageKey).HasMaxLength(500).IsRequired();
        builder.Property(document => document.ContentType).HasMaxLength(100).IsRequired();
    }

    public void Configure(EntityTypeBuilder<NikahWorkflowDecision> builder)
    {
        builder.ToTable("NikahWorkflowDecisions");
        builder.Property(decision => decision.Stage).HasConversion<string>().HasMaxLength(50);
        builder.Property(decision => decision.Outcome).HasConversion<string>().HasMaxLength(50);
        builder.Property(decision => decision.Comment).HasMaxLength(2000).IsRequired();
    }

    public void Configure(EntityTypeBuilder<NikahCorrectionRequest> builder)
    {
        builder.ToTable("NikahCorrectionRequests");
        builder.Property(request => request.RequestedByStage).HasConversion<string>().HasMaxLength(50);
        builder.Property(request => request.Comment).HasMaxLength(2000).IsRequired();
        builder.HasMany(request => request.Fields)
            .WithOne(field => field.NikahCorrectionRequest)
            .HasForeignKey(field => field.NikahCorrectionRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<NikahCorrectionField> builder)
    {
        builder.ToTable("NikahCorrectionFields");
        builder.Property(field => field.FieldKey).HasMaxLength(150).IsRequired();
        builder.HasIndex(field => new { field.NikahCorrectionRequestId, field.FieldKey }).IsUnique();
    }

    public void Configure(EntityTypeBuilder<NikahCertificate> builder)
    {
        builder.ToTable("NikahCertificates");
        builder.Property(certificate => certificate.SerialNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(certificate => certificate.SerialNumber).IsUnique();
        builder.HasIndex(certificate => new { certificate.NikahApplicationId, certificate.Revision }).IsUnique();
        builder.Property(certificate => certificate.SnapshotJson).IsRequired();
        builder.Property(certificate => certificate.PdfStorageKey).HasMaxLength(500);
        builder.HasOne(certificate => certificate.ReplacesCertificate)
            .WithMany(certificate => certificate.ReplacementCertificates)
            .HasForeignKey(certificate => certificate.ReplacesCertificateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
