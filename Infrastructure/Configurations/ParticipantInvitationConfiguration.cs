using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ParticipantInvitationConfiguration
        : IEntityTypeConfiguration<ParticipantInvitation>
    {
        public void Configure(EntityTypeBuilder<ParticipantInvitation> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ApplicationId).IsRequired();

            builder.HasOne(p => p.Application)
                .WithMany()
                .HasForeignKey(p => p.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.TokenHash)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(p => p.TokenHash);
            builder.HasIndex(p => p.ApplicationId);
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.ExpiresAt);

            builder.Property(p => p.WitnessOrder).IsRequired(false);
        }
    }
}
