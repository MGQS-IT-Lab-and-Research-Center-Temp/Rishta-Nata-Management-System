using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class BrideConfiguration : IEntityTypeConfiguration<Bride>
{
    public void Configure(EntityTypeBuilder<Bride> builder)
    {
        builder.ToTable("Brides");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MembershipNo)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(x => x.MarriageApplicationForm)
            .WithOne(x => x.Bride)
            .HasForeignKey<Bride>(x => x.MarriageApplicationFormId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.MarriageApplicationForm)
               .WithOne(f => f.Bride)
               .HasForeignKey<Bride>(b => b.MarriageApplicationFormId);
    }
}
