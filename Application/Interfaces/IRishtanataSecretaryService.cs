using Infrastructure.DTOs.JamaatMember;
using Infrastructure.DTOs.MarriedCoupleDto;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;

namespace Application.Interfaces;

/// <summary>
/// National Rishtanata Secretary: dashboard, pending-approval list, married
/// couples, member lookup, and approve/reject/return actions.
/// Cleanup: file renamed from IRishtanataSecretaryServices.cs (plural) so the
/// filename matches the interface name.
/// </summary>
public interface IRishtanataSecretaryService
{
    RishtanataSecretaryDashboardDto GetDashboard();

    List<PendingApprovalDto> GetPendingApprovals();

    ReviewApplicationDto GetById(Guid id);

    List<MarriedCoupleDto> GetMarriedCouples();
    MemberProfileDto GetMemberProfile(Guid id);

    List<JamaatMemberDto> GetMembers();

    // Cleanup: were `void` fire-and-forget saves; made Task so callers await
    // the status change before redirecting.
    Task Approve(Guid id);

    Task Reject(Guid id);

    Task ReturnToPresident(Guid id);
}