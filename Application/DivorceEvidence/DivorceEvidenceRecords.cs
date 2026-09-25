using Domain.Enums;

namespace Application.DivorceEvidence;

/// <summary>What the review pages list about a stored certificate.</summary>
public sealed record DivorceEvidenceDocumentInfo(
    DivorceEvidenceParty Party,
    string OriginalFileName,
    long SizeBytes,
    DateTime UploadedAt);

/// <summary>An opened certificate for download. The caller disposes Content.</summary>
public sealed record DivorceEvidenceFile(
    Stream Content,
    string ContentType,
    string FileName);
