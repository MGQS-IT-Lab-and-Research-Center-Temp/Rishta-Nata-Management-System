
using Domain.Entities;

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

        Task<MarriageApplicationForm?> GetByReferenceNumberAsync(
            string referenceNumber,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default);
    }
}