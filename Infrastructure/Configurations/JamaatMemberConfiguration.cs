using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class JamaatMemberConfiguration : IEntityTypeConfiguration<JamaatMember>
{
    public void Configure(EntityTypeBuilder<JamaatMember> builder)
    {
        // Pomelo handles Guid <-> char(36) natively — no converters needed.
        // For Guid? (nullable) properties, EF checks IsDBNull() before GetGuid(),
        // so NULL columns are handled correctly.

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

    }
}
