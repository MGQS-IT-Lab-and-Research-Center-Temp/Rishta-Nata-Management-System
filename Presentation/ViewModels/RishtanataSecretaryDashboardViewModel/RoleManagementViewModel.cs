using Infrastructure.DTOs.Roles;

namespace Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

public class RoleManagementViewModel
{

    public Guid MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ChandaNo { get; set; } = string.Empty;

    public RoleDto CurrentRole { get; set; } = default!;
    public IEnumerable<RoleDto> AvailableRoles { get; set; }

    public bool IsAtBaseRole => CurrentRole.HierarchyLevel == 1;
}
