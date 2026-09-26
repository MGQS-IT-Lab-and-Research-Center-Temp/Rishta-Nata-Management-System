namespace Domain.Interfaces;

/// <summary>
/// Private file store for uploaded documents (Gap 8). Callers choose flat,
/// GUID-based file names; implementations must reject anything that is not a
/// bare file name, so a stored name can never address a path.
/// </summary>
public interface IDocumentStorage
{
    /// <summary>Writes a new file. Fails if the name already exists.</summary>
    Task SaveAsync(string storedFileName, Stream content, CancellationToken cancellationToken = default);

    /// <summary>Opens the file for reading, or returns null when it does not exist.</summary>
    Stream? OpenRead(string storedFileName);

    /// <summary>Deletes the file; does nothing when it does not exist.</summary>
    void Delete(string storedFileName);
}
