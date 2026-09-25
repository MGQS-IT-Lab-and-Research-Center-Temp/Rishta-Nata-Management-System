using Application.DivorceEvidence;
using Domain.Enums;

namespace Application.Interfaces;

/// <summary>
/// Stores and serves the divorce certificates behind Gap 8. Callers validate
/// uploads with DivorceEvidenceRules first. Whether someone may download is
/// IStageAuthorizationService.CanViewFormDocumentsAsync's decision, not this
/// service's.
/// </summary>
public interface IDivorceEvidenceService
{
    Task<bool> HasDocumentAsync(
        Guid formId, DivorceEvidenceParty party,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the file and its row, replacing (and deleting) any earlier upload
    /// for the same form and party, and commits. Returns null on success, or
    /// DivorceEvidenceRules.ConcurrentUploadMessage when a concurrent first
    /// upload for the same form and party won the unique index (nothing is
    /// saved). Throws ArgumentException for an upload that DivorceEvidenceRules
    /// rejects.
    /// </summary>
    Task<string?> SaveAsync(
        Guid formId, DivorceEvidenceParty party, DivorceEvidenceUpload upload,
        string uploadedByMembershipNo,
        CancellationToken cancellationToken = default);

    /// <summary>The stored certificates for a form; the id may be the form's or its application's.</summary>
    Task<IReadOnlyList<DivorceEvidenceDocumentInfo>> ListAsync(
        Guid formOrApplicationId,
        CancellationToken cancellationToken = default);

    /// <summary>Opens one certificate, or returns null when there is none.</summary>
    Task<DivorceEvidenceFile?> OpenAsync(
        Guid formOrApplicationId, DivorceEvidenceParty party,
        CancellationToken cancellationToken = default);
}
