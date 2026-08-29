using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class RishtanataDbContextFactory
    : IDesignTimeDbContextFactory<RishtanataDbContext>
{
    public RishtanataDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var presentationPath = Path.Combine(basePath, "Presentation");
        if (!Directory.Exists(presentationPath))
        {
            presentationPath = Path.Combine(basePath, "..", "Presentation");
        }
        var resolvedPath = Path.GetFullPath(presentationPath);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(resolvedPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection was not found.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<RishtanataDbContext>();
        
        optionsBuilder.UseMySQL(connectionString);

        return new RishtanataDbContext(optionsBuilder.Options);
    }
}