using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    /// <summary>
    /// CRUD for the marriage application form plus signature submissions and
    /// the revert flow (see MarriageApplicationFormService)
    /// </summary>
    public interface IMarriageApplicationFormService
    {
        // Create application
        Task<MarriageApplicationForm> CreateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default);

        // Get application by ID
        Task<MarriageApplicationForm?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        // Get application by MarriageApplicationId
        Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(
            Guid marriageApplicationId);

        // Get application by bridegroom membership number
        Task<MarriageApplicationForm?> GetByMembershipNoAsync(
            string membershipNo,
            CancellationToken cancellationToken = default);

        // Update application
        Task<bool> UpdateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default);

        // Guardian / Wakeel signs
        Task<bool> SubmitGuardianOrWakeelAsync(
            Guid marriageApplicationFormId,
            string signature,
            CancellationToken cancellationToken = default);

        // Witness signs
        Task<bool> SubmitWitnessSignatureAsync(
            Guid marriageApplicationFormId,
            Guid witnessSignatureId,
            string signature,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Reverts a form to an earlier workflow stage and removes data from
        /// stages after the selected target.
        /// </summary>
        Task<RevertStageResult> RevertStageAsync(
            Guid formId,
            ApplicationStage targetStage,
            string reason,
            Guid verifierId,
            CancellationToken cancellationToken = default);
    }
}