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

    /// <summary>
    /// Authorizes against the full paper-form workflow stage
    /// (<see cref="MarriageFormStage"/>) tracked on the form's FormStage
    /// field. Used by the Epic D workflow methods whose stages — e.g.
    /// AwaitingImamVerification, AwaitingWitnesses — have no counterpart in
    /// the review-chain <see cref="ApplicationStage"/> enum.
    /// </summary>
    Task<StageAuthorizationResult> CanUserActAsync(
        Guid userId,
        Guid applicationFormId,
        MarriageFormStage targetStage,
        CancellationToken cancellationToken = default);
}
