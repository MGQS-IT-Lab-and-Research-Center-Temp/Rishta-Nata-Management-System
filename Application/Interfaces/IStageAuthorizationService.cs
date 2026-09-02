using Application.Authorization;
using Domain.Enums;

namespace Application.Interfaces;

/// <summary>
/// The authorization gate defined by docs/stage-authorization-policy.md.
/// Two overloads because the codebase tracks two stage enums (the review-chain
/// ApplicationStage and the paper-form MarriageFormStage).
/// </summary>
public interface IStageAuthorizationService
{
   Task<StageAuthorizationResult> CanUserActAsync(
        Guid userId,
        Guid applicationFormId,
        ApplicationStage targetStage,
        CancellationToken cancellationToken = default);

    /// Authorizes against the full paper-form workflow stage tracked on the form's FormStage field. 
    /// Used by workflow methods whose stages — e.g.
    /// AwaitingImamVerification, AwaitingWitnesses — have no counterpart in the review-chain
    
    Task<StageAuthorizationResult> CanUserActAsync(
        Guid userId,
        Guid applicationFormId,
        MarriageFormStage targetStage,
        CancellationToken cancellationToken = default);
}
