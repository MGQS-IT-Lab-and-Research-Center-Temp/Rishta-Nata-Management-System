using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configurations;

public class JamaatMemberRoleConfiguration : IEntityTypeConfiguration<JamaatMemberRole>
{
    public void Configure(EntityTypeBuilder<JamaatMemberRole> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.JamaatMember)
            .WithMany(m => m.MemberRoles)
            .HasForeignKey(x => x.JamaatMemberId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany(r => r.MemberRoles)
            .HasForeignKey(x => x.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // A member can only hold a given role once.
        builder.HasIndex(x => new { x.JamaatMemberId, x.RoleId }).IsUnique();
    }
}