using Application.Authorization;
using Domain.Entities;
using Infrastructure.DTOs;

namespace Application.Interfaces;

public interface IBrideGuardianService
{
    Task<StageAuthorizationResult> SubmitBrideSectionAsync(
        Guid userId, Guid applicationFormId, BrideSectionDto dto,
        CancellationToken cancellationToken = default);

    Task<BrideGuardian?> CreateAsync(
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