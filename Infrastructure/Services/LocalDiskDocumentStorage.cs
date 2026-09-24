using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services;

/// <summary>
/// IDocumentStorage on the local disk, outside wwwroot (Gap 8). The folder is
/// FileStorage:DivorceEvidencePath; a relative path is resolved against the
/// content root, and an empty value means App_Data/divorce-evidence.
/// </summary>
public class LocalDiskDocumentStorage : IDocumentStorage
{
    private const string DefaultRelativePath = "App_Data/divorce-evidence";

    private readonly string _root;

    public LocalDiskDocumentStorage(IConfiguration configuration, IHostEnvironment environment)
    {
        var configured = configuration["FileStorage:DivorceEvidencePath"];
        var path = string.IsNullOrWhiteSpace(configured) ? DefaultRelativePath : configured.Trim();

        _root = Path.GetFullPath(Path.IsPathRooted(path)
            ? path
            : Path.Combine(environment.ContentRootPath, path));

        Directory.CreateDirectory(_root);
    }

    public async Task SaveAsync(string storedFileName, Stream content, CancellationToken cancellationToken = default)
    {
        var path = ResolvePath(storedFileName);

        await using var file = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(file, cancellationToken);
    }

    public Stream? OpenRead(string storedFileName)
    {
        var path = ResolvePath(storedFileName);

        return File.Exists(path)
            ? new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
            : null;
    }

    public void Delete(string storedFileName)
    {
        var path = ResolvePath(storedFileName);

        if (File.Exists(path))
            File.Delete(path);
    }

    // Only a bare file name is accepted, so a stored name can never climb out
    // of the storage folder.
    private string ResolvePath(string storedFileName)
    {
        if (string.IsNullOrWhiteSpace(storedFileName) ||
            !string.Equals(Path.GetFileName(storedFileName), storedFileName, StringComparison.Ordinal))
        {
            throw new ArgumentException("Invalid stored file name.", nameof(storedFileName));
        }

        return Path.Combine(_root, storedFileName);
    }
}
