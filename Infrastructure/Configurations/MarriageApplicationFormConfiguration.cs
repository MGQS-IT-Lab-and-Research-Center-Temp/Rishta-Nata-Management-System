using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class MarriageApplicationFormConfiguration : IEntityTypeConfiguration<MarriageApplicationForm>
    {
        public void Configure(EntityTypeBuilder<MarriageApplicationForm> builder)
        {
            builder.HasKey(f => f.Id);

            // One form per application.
            builder.Property(f => f.MarriageApplicationId)
                .IsRequired();

            builder.HasIndex(f => f.MarriageApplicationId)
                .IsUnique();

            builder.HasOne<MarriageApplication>()
                .WithOne()
                .HasForeignKey<MarriageApplicationForm>(f => f.MarriageApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(f => f.ReferenceNumber)
                .HasMaxLength(50);

            builder.Property(f => f.Venue)
                .HasMaxLength(200);

            // Bride
            builder.Property(f => f.BrideName).HasMaxLength(200);
            builder.Property(f => f.BrideResidentOf).HasMaxLength(300);
            builder.Property(f => f.BrideGenotype).HasMaxLength(10);
            builder.Property(f => f.BrideBloodGroup).HasMaxLength(10);
            builder.Property(f => f.BrideMaritalStatus).HasMaxLength(50);
            builder.Property(f => f.BrideProposedDowerAmount).HasColumnType("decimal(18,2)");
            builder.Property(f => f.BrideDowerAmountReceivedInCash).HasColumnType("decimal(18,2)");

            // Bridegroom
            builder.Property(f => f.BridegroomName).HasMaxLength(200);
            builder.Property(f => f.BridegroomResidentOf).HasMaxLength(300);
            builder.Property(f => f.BridegroomGenotype).HasMaxLength(10);
            builder.Property(f => f.BridegroomBloodGroup).HasMaxLength(10);
            builder.Property(f => f.BridegroomDowerAmountPaidInCash).HasColumnType("decimal(18,2)");
            builder.Property(f => f.BridegroomDowerAmountToBePaid).HasColumnType("decimal(18,2)");

            // Parents
            builder.Property(f => f.BrideFatherName).HasMaxLength(200);
            builder.Property(f => f.BridegroomFatherName).HasMaxLength(200);

            // Guardian
            builder.Property(f => f.GuardianName).HasMaxLength(200);
            builder.Property(f => f.GuardianRelationToBride).HasMaxLength(100);
            builder.Property(f => f.GuardianAddress).HasMaxLength(300);
            builder.Property(f => f.GuardianTel).HasMaxLength(30);

            // Representative
            builder.Property(f => f.RepresentativeName).HasMaxLength(200);
            builder.Property(f => f.RepresentativeAddress).HasMaxLength(300);
            builder.Property(f => f.RepresentativeActingFor).HasMaxLength(50);

            // Witnesses
            builder.Property(f => f.WitnessOneName).HasMaxLength(200);
            builder.Property(f => f.WitnessOneAddress).HasMaxLength(300);
            builder.Property(f => f.WitnessOneTel).HasMaxLength(30);

            builder.Property(f => f.WitnessTwoName).HasMaxLength(200);
            builder.Property(f => f.WitnessTwoAddress).HasMaxLength(300);
            builder.Property(f => f.WitnessTwoTel).HasMaxLength(30);

            // Verification & Approval
            builder.Property(f => f.OfficiatingImamName).HasMaxLength(200);
            builder.Property(f => f.OfficiatingImamAddressJamaat).HasMaxLength(300);
            builder.Property(f => f.JamaatPresidentName).HasMaxLength(200);
            builder.Property(f => f.NationalRishtanataSecretaryName).HasMaxLength(200);
        }
    }
}