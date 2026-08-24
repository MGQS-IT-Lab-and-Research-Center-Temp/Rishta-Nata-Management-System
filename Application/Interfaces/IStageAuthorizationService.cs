using Application.Authorization;
using Domain.Enums;

namespace Application.Interfaces;

public interface IStageAuthorizationService
{
   Task<StageAuthorizationResult> CanUserActAsync(
        Guid userId,
        Guid applicationFormId,
        ApplicationStage targetStage,
        CancellationToken cancellationToken = default);
}