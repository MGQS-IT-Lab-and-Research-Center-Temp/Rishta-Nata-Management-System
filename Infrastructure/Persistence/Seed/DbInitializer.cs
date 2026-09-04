using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed;

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
