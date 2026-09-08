using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class RishtanataDbContextFactory : IDesignTimeDbContextFactory<RishtanataDbContext>
{
    public RishtanataDbContext CreateDbContext(string[] args)
    {
        DotNetEnv.Env.TraversePath().Load();

        var connectionString = Environment.GetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "ConnectionStrings__DefaultConnection not found. " +
                "Create a .env file at the repository root.");

        var options = new DbContextOptionsBuilder<RishtanataDbContext>()
            .UseMySQL(connectionString)
            .Options;

        return new RishtanataDbContext(options);
    }
}
