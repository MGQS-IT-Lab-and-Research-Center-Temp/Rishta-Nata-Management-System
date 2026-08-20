using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Infrastructure.DTOs.JamaatMember;
public interface IRishtanataSecretaryService
{
    RishtanataSecretaryDashboardDto GetDashboard();

    List<PendingApprovalDto> GetPendingApprovals();

    ReviewApplicationDto GetById(Guid id);

    List<MarriedCoupleDto> GetMarriedCouples();

    List<JamaatMemberDto> GetMembers();

    void Approve(Guid id);

    void Reject(Guid id);

    void ReturnToPresident(Guid id);
}