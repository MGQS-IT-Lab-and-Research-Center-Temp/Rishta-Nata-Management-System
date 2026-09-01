using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface INikahApplicationService
{
    Task<NikahApplication> CreateDraftAsync(NikahApplication application, CancellationToken cancellationToken = default);
    Task<NikahApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SubmitAsync(Guid applicationId, Guid actorId, CancellationToken cancellationToken = default);
    Task RecordDecisionAsync(
        Guid applicationId,
        Guid actorId,
        NikahReviewStage stage,
        NikahReviewOutcome outcome,
        string comment,
        IReadOnlyCollection<string>? correctionFieldKeys = null,
        CancellationToken cancellationToken = default);
    Task ResubmitCorrectionAsync(Guid applicationId, Guid actorId, CancellationToken cancellationToken = default);
}
