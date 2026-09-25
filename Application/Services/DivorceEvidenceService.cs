using Application.DivorceEvidence;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Gap 8: one certificate row per form+party, with the bytes in
/// IDocumentStorage. SaveAsync commits on its own, so section services call
/// it before they modify the form.
/// </summary>
public class DivorceEvidenceService : IDivorceEvidenceService
{
    private const int MaxOriginalFileNameLength = 255;

    private readonly RishtanataDbContext _context;
    private readonly IDocumentStorage _storage;

    public DivorceEvidenceService(RishtanataDbContext context, IDocumentStorage storage)
    {
        _context = context;
        _storage = storage;
    }

    public Task<bool> HasDocumentAsync(
        Guid formId, DivorceEvidenceParty party,
        CancellationToken cancellationToken = default) =>
        _context.DivorceEvidenceDocuments.AnyAsync(
            d => d.MarriageApplicationFormId == formId && d.Party == party,
            cancellationToken);

    public async Task SaveAsync(
        Guid formId, DivorceEvidenceParty party, DivorceEvidenceUpload upload,
        string uploadedByMembershipNo,
        CancellationToken cancellationToken = default)
    {
        var error = DivorceEvidenceRules.Validate(upload);
        if (error is not null)
            throw new ArgumentException(error, nameof(upload));

        var kind = DivorceEvidenceRules.KindOf(upload);
        var storedFileName = $"{Guid.NewGuid():N}{kind.Extension.ToLowerInvariant()}";

        var document = await _context.DivorceEvidenceDocuments
            .FirstOrDefaultAsync(
                d => d.MarriageApplicationFormId == formId && d.Party == party,
                cancellationToken);

        var replacedFileName = document?.StoredFileName;

        if (document is null)
        {
            document = new DivorceEvidenceDocument
            {
                MarriageApplicationFormId = formId,
                Party = party
            };
            _context.DivorceEvidenceDocuments.Add(document);
        }
        else
        {
            document.ModifiedAt = DateTime.UtcNow;
        }

        document.OriginalFileName = SafeOriginalFileName(upload.FileName, kind.Extension);
        document.StoredFileName = storedFileName;
        document.ContentType = kind.ContentType;
        document.SizeBytes = upload.Content.Length;
        document.UploadedAt = DateTime.UtcNow;
        document.UploadedBy = (uploadedByMembershipNo ?? string.Empty).Trim();

        try
        {
            using (var content = new MemoryStream(upload.Content, writable: false))
            {
                await _storage.SaveAsync(storedFileName, content, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            // Nothing references the new file unless the row saved.
            _storage.Delete(storedFileName);
            throw;
        }

        // The row now points at the new file, so the old one is unreferenced.
        if (replacedFileName is not null)
            _storage.Delete(replacedFileName);
    }

    public async Task<IReadOnlyList<DivorceEvidenceDocumentInfo>> ListAsync(
        Guid formOrApplicationId,
        CancellationToken cancellationToken = default)
    {
        var formId = await ResolveFormIdAsync(formOrApplicationId, cancellationToken);
        if (formId is null)
            return Array.Empty<DivorceEvidenceDocumentInfo>();

        return await _context.DivorceEvidenceDocuments
            .AsNoTracking()
            .Where(d => d.MarriageApplicationFormId == formId.Value)
            .OrderBy(d => d.Party)
            .Select(d => new DivorceEvidenceDocumentInfo(
                d.Party, d.OriginalFileName, d.SizeBytes, d.UploadedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<DivorceEvidenceFile?> OpenAsync(
        Guid formOrApplicationId, DivorceEvidenceParty party,
        CancellationToken cancellationToken = default)
    {
        var formId = await ResolveFormIdAsync(formOrApplicationId, cancellationToken);
        if (formId is null)
            return null;

        var document = await _context.DivorceEvidenceDocuments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                d => d.MarriageApplicationFormId == formId.Value && d.Party == party,
                cancellationToken);

        if (document is null)
            return null;

        var content = _storage.OpenRead(document.StoredFileName);

        return content is null
            ? null
            : new DivorceEvidenceFile(content, document.ContentType, document.OriginalFileName);
    }

    private async Task<Guid?> ResolveFormIdAsync(Guid formOrApplicationId, CancellationToken cancellationToken) =>
        await _context.MarriageApplicationForms
            .AsNoTracking()
            .Where(f => f.Id == formOrApplicationId || f.MarriageApplicationId == formOrApplicationId)
            .Select(f => (Guid?)f.Id)
            .FirstOrDefaultAsync(cancellationToken);

    // Keep only the base name the browser sent, capped to the column width
    // (the tail is kept so the extension survives).
    private static string SafeOriginalFileName(string fileName, string extension)
    {
        var name = Path.GetFileName(fileName ?? string.Empty).Trim();

        if (string.IsNullOrEmpty(name))
            name = "divorce-certificate" + extension;

        return name.Length <= MaxOriginalFileNameLength
            ? name
            : name[^MaxOriginalFileNameLength..];
    }
}
