using Application.Authorization;
using Application.Workflow;

namespace Application.Interfaces;

/// <summary>
/// Designated service methods for advancing a marriage application form
/// through its verification/approval chain (backlog D3). These are the ONLY
/// places allowed to write the form's stage forward — controllers must call
/// IStageAuthorizationService themselves, but these methods never trust that
/// blindly: every method re-checks authorization immediately before writing
/// (policy §5, backlog DoD).
///
/// Paper-form sections covered:
///   - Officiating Imam            → SubmitImamVerificationAsync
///   - Jamaat President            → SubmitJamaatPresidentVerificationAsync
///   - National Rishtanata Secretary → SubmitRishtanataRecommendationAsync
///   - National Amir / Missionary  → ApproveByAmirAsync
/// </summary>
public interface IMarriageFormWorkflowService
{
    /// <summary>
    /// Persists the imam's verification and advances the form from
    /// AwaitingImamVerification to AwaitingJamaatPresident.
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> SubmitImamVerificationAsync(
        Guid userId,
        Guid applicationFormId,
        ImamVerificationSubmission submission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the president's verification and advances the form from
    /// AwaitingJamaatPresident to AwaitingRishtanataSecretary.
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> SubmitJamaatPresidentVerificationAsync(
        Guid userId,
        Guid applicationFormId,
        JamaatPresidentVerificationSubmission submission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the national secretary's recommendation and advances the form
    /// from AwaitingRishtanataSecretary to AwaitingAmirApproval.
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> SubmitRishtanataRecommendationAsync(
        Guid userId,
        Guid applicationFormId,
        RishtanataRecommendationSubmission submission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the Amir's final approval, sets ApprovedDateOfNikah, and moves
    /// the form to Completed — locking it against further edits.
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> ApproveByAmirAsync(
        Guid userId,
        Guid applicationFormId,
        AmirApprovalSubmission submission,
        CancellationToken cancellationToken = default);
}