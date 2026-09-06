using Infrastructure.DTOs.MemberDashboard;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;

namespace Application.Interfaces;

/// <summary>
/// Read model for the logged-in member's own portal: their profile, the
/// marriage(s) they are a party to, and the applications they have submitted.
/// </summary>
public interface IMemberDashboardService
{
    Task<MemberDashboardDto> GetDashboardAsync(
        string membershipNo,
        CancellationToken cancellationToken = default);

    Task<List<MemberApplicationDto>> GetApplicationsAsync(
        string membershipNo,
        CancellationToken cancellationToken = default);

    Task<MemberProfileDto?> GetProfileAsync(
        string membershipNo,
        CancellationToken cancellationToken = default);
}
