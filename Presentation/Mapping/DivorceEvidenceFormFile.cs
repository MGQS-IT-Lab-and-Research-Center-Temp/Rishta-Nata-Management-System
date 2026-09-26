using Application.DivorceEvidence;

namespace Presentation.Mapping;

/// <summary>
/// Reads a posted divorce certificate into the Application-layer upload
/// record (Gap 8). The action's request size limit bounds the buffer.
/// </summary>
public static class DivorceEvidenceFormFile
{
    /// <summary>Returns null when no file was chosen.</summary>
    public static async Task<DivorceEvidenceUpload?> ReadAsync(
        IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return null;

        using var buffer = new MemoryStream();
        await file.CopyToAsync(buffer, cancellationToken);

        return new DivorceEvidenceUpload(file.FileName, buffer.ToArray());
    }
}
