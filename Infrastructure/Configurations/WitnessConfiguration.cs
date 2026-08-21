using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class WitnessConfiguration : IEntityTypeConfiguration<Witness>
{
    public void Configure(EntityTypeBuilder<Witness> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Role)
            .IsRequired();

        builder.Property(w => w.WitnessNumber)
            .IsRequired();

        builder.Property(w => w.FullName)
            .HasMaxLength(35);

        builder.Property(w => w.PhoneNumber)
            .HasMaxLength(15);

        builder.Property(w => w.SignatureDate)
            .HasMaxLength(25);

        builder.Property(w => w.InvitationToken)
            .IsRequired()
            .HasMaxLength(35);

        builder.HasIndex(w => w.InvitationToken)
            .IsUnique();

        builder.HasIndex(w => new
        {
            w.MarriageApplicationFormId,
            w.Role,
            w.WitnessNumber
        })
          .IsUnique();

        builder.HasOne(w => w.MarriageApplicationForm)
    .WithMany(f => f.Witnesses)
    .HasForeignKey(w => w.MarriageApplicationFormId)
    .OnDelete(DeleteBehavior.Cascade);
    }
}