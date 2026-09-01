using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class MarriageApplicationFormConfiguration
    : IEntityTypeConfiguration<MarriageApplicationForm>
{
    public void Configure(EntityTypeBuilder<MarriageApplicationForm> builder)
    {
        builder.HasKey(f => f.Id);

        // =====================================================
        // Marriage Application Relationship
        // =====================================================

        builder.Property(f => f.MarriageApplicationId)
            .IsRequired();

        builder.HasIndex(f => f.MarriageApplicationId)
            .IsUnique();

        builder.HasOne(f => f.MarriageApplication)
                .WithOne(a => a.MarriageApplicationForm)
                .HasForeignKey<MarriageApplicationForm>(
     f => f.MarriageApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

        // The inverse side of a stale Certificate relationship that no
        // longer exists in the schema. The live one-to-one runs through
        // FormApplication.Certificate (Certificate is the dependent side);
        // this navigation must be ignored so EF does not try to pair it
        // with a non-existent column (see CertificateConfiguration).
        builder.Ignore(f => f.Certificate);

        // =====================================================
        // Verification & approval section relationships (D3).
        // Configured explicitly so convention discovery cannot pair the
        // shared FK property with more than one relationship.
        // =====================================================

        builder.HasOne(f => f.ImamVerification)
                .WithOne(s => s.MarriageApplicationForm)
                .HasForeignKey<ImamVerificationSection>(
                     s => s.MarriageApplicationFormId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.JamaatPresidentVerification)
                .WithOne(s => s.MarriageApplicationForm)
                .HasForeignKey<JamaatPresidentVerificationSection>(
                     s => s.MarriageApplicationFormId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.RishtanataRecommendation)
                .WithOne(s => s.MarriageApplicationForm)
                .HasForeignKey<RishtanataRecommendationSection>(
                     s => s.MarriageApplicationFormId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.AmirApproval)
                .WithOne(s => s.MarriageApplicationForm)
                .HasForeignKey<AmirApprovalSection>(
                     s => s.MarriageApplicationFormId)
                .OnDelete(DeleteBehavior.Cascade);

        // =====================================================
        // Application
        // =====================================================

        builder.Property(f => f.ReferenceNumber)
            .HasMaxLength(50);

        builder.Property(f => f.Venue)
            .HasMaxLength(200);

        // =====================================================
        // Bride
        // =====================================================

        builder.Property(f => f.BrideMembershipNo)
            .HasMaxLength(50);

        builder.Property(f => f.BrideName)
            .HasMaxLength(200);

        builder.Property(f => f.BrideResidentOf)
            .HasMaxLength(300);

        builder.Property(f => f.BrideGenotype)
            .HasMaxLength(10);

        builder.Property(f => f.BrideBloodGroup)
            .HasMaxLength(10);

        builder.Property(f => f.BrideMaritalStatus)
            .HasMaxLength(50);

        builder.Property(f => f.BrideProposedDowerAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(f => f.BrideDowerAmountReceivedInCash)
            .HasColumnType("decimal(18,2)");

        builder.Property(f => f.BrideSignatureTel)
            .HasMaxLength(30);

        // =====================================================
        // Bridegroom
        // =====================================================

        builder.Property(f => f.BridegroomMembershipNo)
            .HasMaxLength(50);

        builder.Property(f => f.BridegroomName)
            .HasMaxLength(200);

        builder.Property(f => f.BridegroomResidentOf)
            .HasMaxLength(300);

        builder.Property(f => f.BridegroomGenotype)
            .HasMaxLength(10);

        builder.Property(f => f.BridegroomBloodGroup)
            .HasMaxLength(10);

        builder.Property(f => f.BridegroomDowerAmountPaidInCash)
            .HasColumnType("decimal(18,2)");

        builder.Property(f => f.BridegroomDowerAmountToBePaid)
            .HasColumnType("decimal(18,2)");

        builder.Property(f => f.BridegroomSignatureTel)
            .HasMaxLength(30);

        // =====================================================
        // Parents
        // =====================================================

        builder.Property(f => f.BrideFatherName)
            .HasMaxLength(200);

        builder.Property(f => f.BridegroomFatherName)
            .HasMaxLength(200);

        // =====================================================
        // Guardian
        // =====================================================

        builder.Property(f => f.GuardianName)
            .HasMaxLength(200);

        builder.Property(f => f.GuardianRelationToBride)
            .HasMaxLength(100);

        builder.Property(f => f.GuardianAddress)
            .HasMaxLength(300);

        builder.Property(f => f.GuardianTel)
            .HasMaxLength(30);

        builder.Property(f => f.GuardianSignatureDate)
            .HasMaxLength(50);

        // =====================================================
        // Representative
        // =====================================================

        builder.Property(f => f.RepresentativeName)
            .HasMaxLength(200);

        builder.Property(f => f.RepresentativeAddress)
            .HasMaxLength(300);

        builder.Property(f => f.RepresentativeActingFor)
            .HasMaxLength(50);

        builder.Property(f => f.RepresentativeSignatureDate)
            .HasMaxLength(50);

        // =====================================================
        // Witness One
        // =====================================================

        builder.Property(f => f.WitnessOneName)
            .HasMaxLength(200);

        builder.Property(f => f.WitnessOneAddress)
            .HasMaxLength(300);

        builder.Property(f => f.WitnessOneTel)
            .HasMaxLength(30);

        builder.Property(f => f.WitnessOneSignatureDate)
            .HasMaxLength(50);

        // =====================================================
        // Witness Two
        // =====================================================

        builder.Property(f => f.WitnessTwoName)
            .HasMaxLength(200);

        builder.Property(f => f.WitnessTwoAddress)
            .HasMaxLength(300);

        builder.Property(f => f.WitnessTwoTel)
            .HasMaxLength(30);

        builder.Property(f => f.WitnessTwoSignatureDate)
            .HasMaxLength(50);

        // =====================================================
        // Verification & Approval
        // =====================================================

        builder.Property(f => f.OfficiatingImamName)
            .HasMaxLength(200);

        builder.Property(f => f.OfficiatingImamAddressJamaat)
            .HasMaxLength(300);

        builder.Property(f => f.OfficiatingImamSignatureDate)
            .HasMaxLength(50);

        builder.Property(f => f.JamaatPresidentName)
            .HasMaxLength(200);

        builder.Property(f => f.JamaatPresidentSignatureDate)
            .HasMaxLength(50);

        builder.Property(f => f.NationalRishtanataSecretaryName)
            .HasMaxLength(200);

        builder.Property(f => f.NationalRishtanataSecretarySignatureDate)
            .HasMaxLength(50);

        builder.Property(f => f.NationalAmirOrMissionarySignatureDate)
            .HasMaxLength(50);

        // Bride
        builder.HasOne(f => f.BrideSection)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey<BrideFormSection>(
                s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Bridegroom
        builder.HasOne(f => f.BridegroomSection)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey<BridegroomFormSection>(
                s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Guardian / Wakeel
        builder.HasOne(f => f.GuardianOrWakeelSection)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey<GuardianOrWakeelSection>(
                s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Imam
        builder.HasOne(f => f.ImamVerification)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey<ImamVerificationSection>(
                s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Jamaat President
        builder.HasOne(f => f.JamaatPresidentVerification)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey<JamaatPresidentVerificationSection>(
                s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Rishtanata Recommendation
        builder.HasOne(f => f.RishtanataRecommendation)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey<RishtanataRecommendationSection>(
                s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Amir Approval
        builder.HasOne(f => f.AmirApproval)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey<AmirApprovalSection>(
                s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Witnesses
        builder.HasMany(f => f.WitnessSignatures)
            .WithOne(s => s.MarriageApplicationForm)
            .HasForeignKey(s => s.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}