using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Application boundary for the replacement Nikah aggregate. Role/office
/// authorization is performed by the calling endpoint; this service enforces
/// the aggregate's state and audit invariants immediately before persistence.
/// </summary>
public class NikahApplicationService : INikahApplicationService
{
    private readonly RishtanataDbContext _context;

    public NikahApplicationService(RishtanataDbContext context) => _context = context;

    public async Task<NikahApplication> CreateDraftAsync(NikahApplication application, CancellationToken cancellationToken = default)
    {
        application.Id = application.Id == Guid.Empty ? Guid.NewGuid() : application.Id;
        application.CreatedAt = DateTime.UtcNow;
        _context.NikahApplications.Add(application);
        await _context.SaveChangesAsync(cancellationToken);
        return application;
    }

    public Task<NikahApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        LoadAggregate(id).FirstOrDefaultAsync(application => application.Id == id, cancellationToken);

    public async Task SubmitAsync(Guid applicationId, Guid actorId, CancellationToken cancellationToken = default)
    {
        var application = await RequireAggregate(applicationId, cancellationToken);
        if (application.PrimaryApplicantUserId != actorId)
            throw new UnauthorizedAccessException("Only the primary applicant can submit this application.");

        application.Submit(DateTime.UtcNow);
        Stamp(application, actorId);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RecordDecisionAsync(
        Guid applicationId,
        Guid actorId,
        NikahReviewStage stage,
        NikahReviewOutcome outcome,
        string comment,
        IReadOnlyCollection<string>? correctionFieldKeys = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Every review decision requires a comment.", nameof(comment));

        var application = await RequireAggregate(applicationId, cancellationToken);
        var now = DateTime.UtcNow;
        application.WorkflowDecisions.Add(new NikahWorkflowDecision
        {
            Id = Guid.NewGuid(),
            Stage = stage,
            Outcome = outcome,
            Comment = comment.Trim(),
            DecidedAt = now,
            CreatedAt = now,
            CreatedBy = actorId
        });

        if (outcome == NikahReviewOutcome.Approved)
        {
            application.AdvanceAfterApproval(stage, now);
        }
        else
        {
            var keys = correctionFieldKeys?.Where(key => !string.IsNullOrWhiteSpace(key)).Distinct(StringComparer.Ordinal).ToArray()
                ?? Array.Empty<string>();
            if (keys.Length == 0)
                throw new ArgumentException("A correction request must select at least one field.", nameof(correctionFieldKeys));

            var request = new NikahCorrectionRequest
            {
                Id = Guid.NewGuid(),
                RequestedByStage = stage,
                Comment = comment.Trim(),
                RequestedAt = now,
                CreatedAt = now,
                CreatedBy = actorId
            };
            foreach (var key in keys)
            {
                request.Fields.Add(new NikahCorrectionField
                {
                    Id = Guid.NewGuid(),
                    FieldKey = key.Trim(),
                    CreatedAt = now,
                    CreatedBy = actorId
                });
            }
            application.CorrectionRequests.Add(request);
            application.RequestCorrection(stage);
        }

        Stamp(application, actorId);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ResubmitCorrectionAsync(Guid applicationId, Guid actorId, CancellationToken cancellationToken = default)
    {
        var application = await RequireAggregate(applicationId, cancellationToken);
        if (application.PrimaryApplicantUserId != actorId)
            throw new UnauthorizedAccessException("Only the primary applicant can resubmit a correction.");

        application.ResubmitCorrection();
        var pendingRequest = application.CorrectionRequests
            .Where(request => request.ResubmittedAt is null)
            .OrderByDescending(request => request.RequestedAt)
            .FirstOrDefault();
        if (pendingRequest is not null)
        {
            pendingRequest.ResubmittedAt = DateTime.UtcNow;
            pendingRequest.ModifiedAt = DateTime.UtcNow;
            pendingRequest.ModifiedBy = actorId;
        }
        Stamp(application, actorId);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<NikahApplication> LoadAggregate(Guid id) => _context.NikahApplications
        .Include(application => application.Bride)
        .Include(application => application.Bridegroom)
        .Include(application => application.GuardianRepresentation)
        .Include(application => application.Witnesses)
        .Include(application => application.Documents)
        .Include(application => application.WorkflowDecisions)
        .Include(application => application.CorrectionRequests).ThenInclude(request => request.Fields)
        .Include(application => application.Certificates)
        .Where(application => application.Id == id);

    private async Task<NikahApplication> RequireAggregate(Guid id, CancellationToken cancellationToken) =>
        await LoadAggregate(id).SingleOrDefaultAsync(cancellationToken)
        ?? throw new KeyNotFoundException("Nikah application was not found.");

    private static void Stamp(NikahApplication application, Guid actorId)
    {
        application.ModifiedAt = DateTime.UtcNow;
        application.ModifiedBy = actorId;
    }
}
