using Infrastructure.DTOs.JamaatMember;
using Infrastructure.DTOs.MarriedCoupleDto;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;

namespace Application.Interfaces;

public interface IRishtanataSecretaryService
{
    RishtanataSecretaryDashboardDto GetDashboard();

    List<PendingApprovalDto> GetPendingApprovals();

    ReviewApplicationDto GetById(Guid id);

    List<MarriedCoupleDto> GetMarriedCouples();
    MemberProfileDto GetMemberProfile(Guid id);

    List<JamaatMemberDto> GetMembers();

    void Approve(Guid id);

    void Reject(Guid id);

    void ReturnToPresident(Guid id);
}