using Application.Authorization;
using Application.Workflow;

namespace Application.Interfaces;

/// <summary>
/// Designated service methods for advancing a marriage application form
/// through its verification/approval chain (backlog D3). Safe-guard writers of
/// the form's stage forward on the verification/approval chain (the filling
/// phase stages are written elsewhere). Controllers must call
/// IStageAuthorizationService themselves, but these methods never trust that
/// blindly: every method re-checks authorization immediately before writing
/// (policy §5, backlog DoD).
///
/// Chain (Imam signs AFTER the ceremony, not during filling):
///   - Jamaat President (bride's, or shared Jama'at) → SubmitJamaatPresidentVerificationAsync
///   - Jamaat President (groom's — different Jama'ats only) → SubmitGroomJamaatPresidentVerificationAsync
///   - National Rishtanata Secretary → SubmitRishtanataRecommendationAsync
///   - National Amir / Missionary → ApproveByAmirAsync
///   - Officiating Imam (post-ceremony) → SubmitImamSignoffAsync
/// </summary>
public interface IMarriageFormWorkflowService
{
    /// <summary>
    /// Persists the bride's Jama'at President (or the shared-Jama'at president)
    /// sign-off and advances the form from AwaitingBrideJamaatPresident to either
    /// AwaitingGroomJamaatPresident (different Jama'ats) or
    /// AwaitingRishtanataSecretary (same Jama'at).
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> SubmitJamaatPresidentVerificationAsync(
        string membershipNo,
        Guid applicationFormId,
        JamaatPresidentVerificationSubmission submission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the groom's Jama'at President sign-off and advances the form from
    /// AwaitingGroomJamaatPresident to AwaitingRishtanataSecretary.
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> SubmitGroomJamaatPresidentVerificationAsync(
        string membershipNo,
        Guid applicationFormId,
        JamaatPresidentVerificationSubmission submission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the national secretary's recommendation (including the designated
    /// officiating imam) and advances the form from AwaitingRishtanataSecretary to
    /// AwaitingAmirApproval.
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> SubmitRishtanataRecommendationAsync(
        string membershipNo,
        Guid applicationFormId,
        RishtanataRecommendationSubmission submission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the Amir's final approval and advances the form to
    /// AwaitingImamSignoff (NOT Completed — the ceremony + imam sign-off come
    /// next). The agreed Nikah date defaults to the couple's proposed date.
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> ApproveByAmirAsync(
        string membershipNo,
        Guid applicationFormId,
        AmirApprovalSubmission submission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the designated imam's post-ceremony sign-off, mirrors it onto the
    /// flat OfficiatingImam* columns, and advances the form from
    /// AwaitingImamSignoff to Completed (locking it).
    /// Denied requests produce no side effects.
    /// </summary>
    Task<StageAuthorizationResult> SubmitImamSignoffAsync(
        string membershipNo,
        Guid applicationFormId,
        ImamSignoffSubmission submission,
        CancellationToken cancellationToken = default);
}