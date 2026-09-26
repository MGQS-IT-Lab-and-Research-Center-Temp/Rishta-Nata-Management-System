namespace Application.DivorceEvidence;

/// <summary>
/// A divorce certificate as posted by an applicant (Gap 8). Content is the
/// whole file; the request size limit keeps it small enough to buffer.
/// </summary>
public sealed record DivorceEvidenceUpload(string FileName, byte[] Content);
