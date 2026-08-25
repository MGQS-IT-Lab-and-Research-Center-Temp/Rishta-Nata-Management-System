using Domain.Entities;

namespace Application.Interfaces
{
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
    }
}