using Infrastructure.DTOs;

namespace Application.Interfaces
{
    public interface IWitnessFormService    
    {
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
