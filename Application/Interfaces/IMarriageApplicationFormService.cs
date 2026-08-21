using Domain.Entities;
using Infrastructure.DTOs;

namespace Application.Interfaces
{
    public interface IMarriageApplicationFormService
    {
        // Marriage Application Form
        Task<MarriageApplicationForm> CreateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default);

        Task<MarriageApplicationForm?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(
            Guid marriageAplicationId);

        Task<bool> UpdateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default);

        // Witness
        Task<WitnessDto> AddWitnessAsync(
            WitnessDto witness,
            CancellationToken cancellationToken = default);

        Task<WitnessDto?> GetWitnessByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<WitnessDto?> GetWitnessByTokenAsync(
            string token,
            CancellationToken cancellationToken = default);

        Task<bool> CompleteWitnessAsync(
            WitnessDto witness,
          CancellationToken cancellationToken = default);
    }
}
    