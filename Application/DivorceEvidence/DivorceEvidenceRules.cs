namespace Application.DivorceEvidence;

/// <summary>
/// The upload policy for divorce certificates (Gap 8): PDF, JPG or PNG,
/// checked by extension and by magic bytes, at most 5 MB.
/// </summary>
public static class DivorceEvidenceRules
{
    public const long MaxFileBytes = 5 * 1024 * 1024;

    // Multipart cap for the Create/Continue posts: the file plus the form's
    // text fields.
    public const long MaxRequestBytes = 6 * 1024 * 1024;

    public const string AcceptAttribute = ".pdf,.jpg,.jpeg,.png";

    public const string BrideMissingMessage =
        "You declared that you are divorced. Upload the Khula (divorce) certificate as a PDF, JPG or PNG file.";

    public const string GroomMissingMessage =
        "You declared that you divorced a former wife. Upload the Talaq (divorce) certificate as a PDF, JPG or PNG file.";

    private static readonly byte[] PdfSignature = { 0x25, 0x50, 0x44, 0x46, 0x2D };
    private static readonly byte[] JpegSignature = { 0xFF, 0xD8, 0xFF };
    private static readonly byte[] PngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

    private static readonly DivorceEvidenceFileKind[] Kinds =
    {
        new(".pdf", "application/pdf", PdfSignature),
        new(".jpg", "image/jpeg", JpegSignature),
        new(".jpeg", "image/jpeg", JpegSignature),
        new(".png", "image/png", PngSignature)
    };

    /// <summary>Returns an error message, or null when the upload is acceptable.</summary>
    public static string? Validate(DivorceEvidenceUpload upload)
    {
        if (upload.Content.Length == 0)
            return "The divorce certificate file is empty.";

        if (upload.Content.Length > MaxFileBytes)
            return "The divorce certificate must be 5 MB or smaller.";

        var kind = FindKind(upload.FileName);
        if (kind is null)
            return "The divorce certificate must be a PDF, JPG or PNG file.";

        var header = new ReadOnlySpan<byte>(upload.Content);
        if (!header.StartsWith(new ReadOnlySpan<byte>(kind.Signature)))
            return $"The file's contents are not a valid {kind.Extension.TrimStart('.').ToUpperInvariant()} file.";

        return null;
    }

    /// <summary>The stored extension and content type of an upload that passed Validate.</summary>
    public static DivorceEvidenceFileKind KindOf(DivorceEvidenceUpload upload) =>
        FindKind(upload.FileName)
        ?? throw new ArgumentException("Unsupported divorce certificate type.", nameof(upload));

    private static DivorceEvidenceFileKind? FindKind(string? fileName)
    {
        var extension = Path.GetExtension(fileName ?? string.Empty);

        return Kinds.FirstOrDefault(k =>
            string.Equals(k.Extension, extension, StringComparison.OrdinalIgnoreCase));
    }
}

public sealed record DivorceEvidenceFileKind(string Extension, string ContentType, byte[] Signature);
