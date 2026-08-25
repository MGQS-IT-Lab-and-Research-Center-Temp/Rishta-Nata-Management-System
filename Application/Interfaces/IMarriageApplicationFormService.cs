using Domain.Entities;
using Domain.Enums;


namespace Application.Interfaces
{
    public interface IMarriageApplicationFormService
    {
        Task<MarriageApplicationForm> CreateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default);

        Task<MarriageApplicationForm?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(
            Guid marriageAplicationId);

        //Task<MarriageApplicationForm?> GetByReferenceNumberAsync(
        //    string referenceNumber,
        //    CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default);

        Task<MarriageApplicationForm?> GetByMembershipNoAsync(
            string membershipNo,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Reverts a form to an earlier workflow stage and removes data from
        /// stages after the selected target.
        /// </summary>
        Task<bool> RevertStageAsync(
            Guid formId,
            ApplicationStage targetStage,
            string reason,
            Guid verifierId,
            CancellationToken cancellationToken = default);
    }
}
