using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
   

namespace Infrastructure.Configurations
    {
        public class BrideConfiguration : IEntityTypeConfiguration<Bride>
        {
            public void Configure(EntityTypeBuilder<Bride> builder)
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(x => x.DateOfBirth)
                    .IsRequired();

                builder.Property(x => x.Address)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(x => x.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11);

                builder.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(x => x.MaritalStatus)
                    .IsRequired();

                builder.HasOne(x => x.MarriageApplication)
                    .WithOne()
                    .HasForeignKey<Bride>(x => x.MarriageApplicationId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(x => x.MarriageApplicationId)
                    .IsUnique();
            }
        }
 }
