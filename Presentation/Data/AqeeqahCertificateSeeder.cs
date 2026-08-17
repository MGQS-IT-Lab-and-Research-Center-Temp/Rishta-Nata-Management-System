using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Data
{
    public static class AqeeqahCertificateSeeder
    {
        public static async Task SeedAqeeqahCertificatesAsync(RishtanataDbContext context)
        {
            // Check if data already exists
            if (await context.AqeeqahCertificates.AnyAsync())
            {
                return; // Data already seeded
            }

            var certificates = new List<AqeeqahCertificate>
            {
                new AqeeqahCertificate
                {
                    Id = Guid.NewGuid(),
                    SerialNumber = "AQH-2026-001",
                    ChildName = "Muhammad Ali Khan",
                    FatherName = "Ahmed Khan",
                    MotherName = "Fatima Khan",
                    DateOfBirth = new DateTime(2025, 6, 15),
                    Gender = "Male",
                    AqeeqahDate = new DateTime(2025, 7, 20),
                    AqeeqahLocation = "Central Mosque, Karachi",
                    AnimalCount = 2,
                    IssueDate = new DateTime(2025, 7, 25),
                    IssuedByUserId = Guid.NewGuid(),
                    JamaatId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid()
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
                    AqeeqahLocation = "South Karachi Jamaat",
                    AnimalCount = 1,
                    IssueDate = new DateTime(2025, 7, 10),
                    IssuedByUserId = Guid.NewGuid(),
                    JamaatId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid()
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
                    AqeeqahLocation = "North Karachi Community Center",
                    AnimalCount = 2,
                    IssueDate = new DateTime(2025, 6, 5),
                    IssuedByUserId = Guid.NewGuid(),
                    JamaatId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid()
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
                    AqeeqahLocation = "East Side Mosque",
                    AnimalCount = 1,
                    IssueDate = new DateTime(2025, 5, 1),
                    IssuedByUserId = Guid.NewGuid(),
                    JamaatId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid()
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
                    AqeeqahLocation = "West Jamaat Hall",
                    AnimalCount = 2,
                    IssueDate = new DateTime(2025, 4, 15),
                    IssuedByUserId = Guid.NewGuid(),
                    JamaatId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid()
                }
            };

            await context.AqeeqahCertificates.AddRangeAsync(certificates);
            await context.SaveChangesAsync();
        }
    }
}
