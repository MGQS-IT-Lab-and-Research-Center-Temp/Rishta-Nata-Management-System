using Domain.Enums;

namespace Application.Workflow;

/// <summary>
/// Maps between the two stage enums the codebase tracks on a form:
///   - <see cref="MarriageFormStage"/> — the fine-grained paper-form workflow
///     (advances as each section is submitted/verified).
///   - <see cref="ApplicationStage"/> — the coarse review chain used by the
///     revert/rejection flow and the ApplicationStage authorization overload.
///
/// These two progressions were drifting independently (ApplicationStage was
/// initialised once and never advanced), which deadlocked revert. This mapping
/// is the single source of truth that keeps them in sync. Update the reverse
/// mapping when adding a new stage.
///
/// Chain order (Imam signs AFTER the ceremony, not during filling):
///   AwaitingBride/AwaitingBridegroom/AwaitingWitnesses
///     → AwaitingBrideJamaatPresident → [AwaitingGroomJamaatPresident]
///     → AwaitingRishtanataSecretary → AwaitingAmirApproval
///     → AwaitingImamSignoff → Completed
/// </summary>
public static class WorkflowStageMapping
{
    /// <summary>Maps a fine-grained FormStage to its coarse review-chain stage.</summary>
    public static ApplicationStage? ToApplicationStage(MarriageFormStage stage) =>
        stage switch
        {
            MarriageFormStage.AwaitingBride or
            MarriageFormStage.AwaitingBridegroom or
            MarriageFormStage.AwaitingWitnesses =>
                ApplicationStage.ApplicantsReview,

            MarriageFormStage.AwaitingBrideJamaatPresident or
            MarriageFormStage.AwaitingGroomJamaatPresident =>
                ApplicationStage.JamaatPresidentReview,

            MarriageFormStage.AwaitingRishtanataSecretary =>
                ApplicationStage.NationalRishtanataSecretaryVerification,

            MarriageFormStage.AwaitingAmirApproval =>
                ApplicationStage.AmirApproval,

            MarriageFormStage.AwaitingImamSignoff =>
                ApplicationStage.ImamSignoff,

            // Completed and AwaitingApplicants have no coarse counterpart (the
            // obsolete pre-ceremony Imam-verification stage maps to null too).
            _ => null
        };

    /// <summary>
    /// Maps a coarse review-chain stage to the FormStage a revert should land
    /// on. Reverting to the earliest coarse stage sends the form back to the
    /// signature/applicant phase (awaiting witnesses), i.e. before verification.
    /// </summary>
    public static MarriageFormStage ToFormStage(ApplicationStage stage) =>
        stage switch
        {
            ApplicationStage.ApplicantsReview =>
                MarriageFormStage.AwaitingWitnesses,

            ApplicationStage.JamaatPresidentReview =>
                MarriageFormStage.AwaitingBrideJamaatPresident,

            ApplicationStage.NationalRishtanataSecretaryVerification =>
                MarriageFormStage.AwaitingRishtanataSecretary,

            ApplicationStage.AmirApproval =>
                MarriageFormStage.AwaitingAmirApproval,

            ApplicationStage.ImamSignoff =>
                MarriageFormStage.AwaitingImamSignoff,

            _ => throw new ArgumentOutOfRangeException(
                nameof(stage), stage, "Unknown application stage.")
        };
}
