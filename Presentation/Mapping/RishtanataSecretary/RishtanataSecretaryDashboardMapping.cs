using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels;

namespace Presentation.Mapping.RishtanataSecretary;

public static class RishtanataSecretaryDashboardMapping
{
    public static RishtanataSecretaryDashboardViewModel ToViewModel(
        RishtanataSecretaryDashboardDto dto)
    {
        return new RishtanataSecretaryDashboardViewModel
        {
            SecretaryName = dto.SecretaryName,
            PendingApprovals = dto.PendingApprovals,
            ApprovedApplications = dto.ApprovedApplications,
            RejectedApplications = dto.RejectedApplications,
            MarriedCouples = dto.MarriedCouples,
            TotalMembers = dto.TotalMembers
        };
    }
}
