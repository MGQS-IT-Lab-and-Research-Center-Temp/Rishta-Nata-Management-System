using Infrastructure.DTOs.JamaatMember;
using Infrastructure.DTOs.MarriedCoupleDto;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces;

/// <summary>
/// National Rishtanata Secretary: dashboard, pending-approval list, married
/// couples, member lookup, and approve/reject/return actions.
/// Cleanup: file renamed from IRishtanataSecretaryServices.cs (plural) so the
/// filename matches the interface name.
/// </summary>
public interface IRishtanataSecretaryService
{
    RishtanataSecretaryDashboardDto GetDashboard(string? membershipNo);

    List<PendingApprovalDto> GetPendingApprovals();

    ReviewApplicationDto? GetById(Guid id);

    List<MarriedCoupleDto> GetMarriedCouples();
    MemberProfileDto? GetMemberProfile(Guid id);

    List<JamaatMemberDto> GetMembers();

    // Cleanup: were `void` fire-and-forget saves; made Task so callers await
    // the status change before redirecting. Return false when the application
    // could not be found instead of throwing.
    Task<bool> Approve(Guid id);

    Task<bool> Reject(Guid id);

    Task<bool> ReturnToPresident(Guid id);

    /// <summary>
    /// Records the officiating imam (and any partner-communicated agreed-date
    /// change) the National Rishtanata office designates for the application.
    /// </summary>
    Task<bool> UpdateImamDesignationAsync(
        Guid id,
        string officiatingImamMembershipNo,
        DateTime? approvedDateOfNikah,
        CancellationToken cancellationToken = default);
}