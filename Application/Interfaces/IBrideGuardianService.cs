using Domain.Entities;

namespace Application.Interfaces;

public interface IBrideGuardianService
{
    Task<BrideGuardian> CreateAsync(
        BrideGuardian guardian,
        CancellationToken cancellationToken = default);

    Task<BrideGuardian?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<BrideGuardian?> GetByBrideIdAsync(
        Guid brideId,
        CancellationToken cancellationToken = default);

    Task<bool> AssignToBrideAsync(
        Guid guardianId,
        Guid brideId,
        CancellationToken cancellationToken = default);

    Task<BrideGuardian?> GetByMarriageApplicationIdAsync(
        Guid marriageApplicationId,
        CancellationToken cancellationToken = default);

    // Task<bool> UpdateAsync(
    //     BrideGuardian guardian,
    //     CancellationToken cancellationToken = default);
}