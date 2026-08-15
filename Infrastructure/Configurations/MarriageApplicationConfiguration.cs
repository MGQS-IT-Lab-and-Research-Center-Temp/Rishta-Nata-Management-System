using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class MarriageApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(ma => ma.Id);

            builder.Property(ma => ma.Status)
                .HasConversion<string>()
                .IsRequired();

            //builder.Property(ma => ma.UserId)
            //    .IsRequired();
        }
    }
}