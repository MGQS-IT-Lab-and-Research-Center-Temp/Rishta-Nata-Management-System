using Infrastructure.DTOs.JamaatPresidentDashboardDto;

namespace Application.Interfaces;

/// <summary>
/// Jamaat (branch) President dashboard and review actions.
/// </summary>
public interface IJamaatPresidentService
{
    Task<JamaatPresidentDashboardDto> GetDashboardAsync(
        Guid? currentUserId);

    Task<List<NikahApplicationDto>> GetPendingApplicationsAsync(
        Guid? currentUserId);

    Task<List<NikahApplicationDto>> GetReviewedApplicationsAsync(
        Guid? currentUserId);

    Task<JamaatPresidentReviewDto?> GetReviewByIdAsync(Guid id);

    Task<bool> ApproveAsync(Guid id, Guid? currentUserId);

    Task<bool> RejectAsync(Guid id, Guid? currentUserId);

    Task<bool> RequestMoreInformationAsync(Guid id, Guid? currentUserId);
}
