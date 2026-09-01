using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Data;

public static class AqeeqahCertificateSeeder
{
    public static async Task SeedAqeeqahCertificatesAsync(
        RishtanataDbContext context)
    {
        // Do not seed again if certificates already exist
        if (await context.AqeeqahCertificates.AnyAsync())
        {
            return;
        }

        // Get an existing Jamaat member/user to use as IssuedByUserId
        var issuer = await context.JamaatMembers
            .FirstOrDefaultAsync();

        if (issuer == null)
        {
            return;
        }

        // Get an existing Jamaat ID from your members
        var jamaatId = issuer.Id;

        var certificates = new List<AqeeqahCertificate>
        {
            new() {
                Id = Guid.NewGuid(),

                SerialNumber = "AQH-2026-001",

                ChildName = "Muhammad Ali Khan",
                FatherName = "Ahmed Khan",
                MotherName = "Fatima Khan",

                DateOfBirth = new DateTime(2025, 6, 15),
                Gender = "Male",

                AqeeqahDate = new DateTime(2025, 7, 20),
                AqeeqahLocation = "Central Mosque",
                AnimalCount = 2,

                IssueDate = new DateTime(2025, 7, 25),

                IssuedByUserId = issuer.Id,
                JamaatId = jamaatId,

                CertificateFilePath = null,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = issuer.Id
            },

            new AqeeqahCertificate
            {
                Id = Guid.NewGuid(),

                SerialNumber = "AQH-2026-002",

                ChildName = "Aisha Malik",
                FatherName = "Hassan Malik",
                MotherName = "Amina Malik",

                DateOfBirth = new DateTime(2025, 5, 22),
                Gender = "Female",

                AqeeqahDate = new DateTime(2025, 6, 28),
                AqeeqahLocation = "South Jamaat",
                AnimalCount = 1,

                IssueDate = new DateTime(2025, 7, 10),

                IssuedByUserId = issuer.Id,
                JamaatId = jamaatId,

                CertificateFilePath = null,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = issuer.Id
            },

            new AqeeqahCertificate
            {
                Id = Guid.NewGuid(),

                SerialNumber = "AQH-2026-003",

                ChildName = "Omar Abdullah",
                FatherName = "Abdullah Siddiqui",
                MotherName = "Noor Siddiqui",

                DateOfBirth = new DateTime(2025, 4, 10),
                Gender = "Male",

                AqeeqahDate = new DateTime(2025, 5, 15),
                AqeeqahLocation = "North Jamaat",
                AnimalCount = 2,

                IssueDate = new DateTime(2025, 6, 5),

                IssuedByUserId = issuer.Id,
                JamaatId = jamaatId,

                CertificateFilePath = null,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = issuer.Id
            },

            new AqeeqahCertificate
            {
                Id = Guid.NewGuid(),

                SerialNumber = "AQH-2026-004",

                ChildName = "Sara Hussain",
                FatherName = "Hussain Ahmed",
                MotherName = "Maryam Ahmed",

                DateOfBirth = new DateTime(2025, 3, 18),
                Gender = "Female",

                AqeeqahDate = new DateTime(2025, 4, 20),
                AqeeqahLocation = "East Jamaat",
                AnimalCount = 1,

                IssueDate = new DateTime(2025, 5, 1),

                IssuedByUserId = issuer.Id,
                JamaatId = jamaatId,

                CertificateFilePath = null,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = issuer.Id
            },

            new AqeeqahCertificate
            {
                Id = Guid.NewGuid(),

                SerialNumber = "AQH-2026-005",

                ChildName = "Ibrahim Rashid",
                FatherName = "Rashid Khan",
                MotherName = "Zahra Khan",

                DateOfBirth = new DateTime(2025, 2, 28),
                Gender = "Male",

                AqeeqahDate = new DateTime(2025, 3, 30),
                AqeeqahLocation = "West Jamaat",
                AnimalCount = 2,

                IssueDate = new DateTime(2025, 4, 15),

                IssuedByUserId = issuer.Id,
                JamaatId = jamaatId,

                CertificateFilePath = null,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = issuer.Id
            }
        };

        await context.AqeeqahCertificates.AddRangeAsync(certificates);

        await context.SaveChangesAsync();
    }
}

public static class DbInitializer
{
    public static async Task InitializeAsync(
        RishtanataDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        await AqeeqahCertificateSeeder
            .SeedAqeeqahCertificatesAsync(dbContext);
    }
}