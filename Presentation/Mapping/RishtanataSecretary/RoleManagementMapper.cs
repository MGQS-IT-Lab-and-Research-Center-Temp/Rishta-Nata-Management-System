using Infrastructure.DTOs;
using Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

namespace Presentation.Mapping.RishtanataSecretary;

public static class RoleManagementMapper
{
    public static RoleManagementViewModel toViewModel(RoleManagementDto dto)
    {
        return new RoleManagementViewModel
        {
            MemberId = dto.MemberId,
            ChandaNo = dto.ChandaNo,
            FullName = dto.FullName,
            CurrentRole = dto.CurrentRole,
            AvailableRoles = dto.AvailableRoles

        };
    }

}
