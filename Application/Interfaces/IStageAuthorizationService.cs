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
        string membershipNo,
        Guid applicationFormId,
        ApplicationStage targetStage,
        CancellationToken cancellationToken = default);

    /// Authorizes against the full paper-form workflow stage tracked on the form's FormStage field.
    /// Used by workflow methods whose stages — e.g. AwaitingApplicants and
    /// Completed — have no counterpart in the review-chain
    
    Task<StageAuthorizationResult> CanUserActAsync(
        string membershipNo,
        Guid applicationFormId,
        MarriageFormStage targetStage,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// May this member download the form's uploaded documents (Gap 8)? Allowed:
    /// the bride and bridegroom, the Jama'at President of either partner's
    /// Jama'at, the National Rishtanata Secretary, and the Amir/Missionary In
    /// Charge. Not stage-gated: completed forms stay readable.
    /// </summary>
    Task<StageAuthorizationResult> CanViewFormDocumentsAsync(
        string membershipNo,
        Guid applicationFormId,
        CancellationToken cancellationToken = default);
}
