using Infrastructure.DTOs.Roles;

namespace Infrastructure.DTOs;

public class RoleManagementDto
{
    public Guid MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ChandaNo { get; set; } = string.Empty;

    public RoleDto CurrentRole { get; set; } = default!;
    public IEnumerable<RoleDto> AvailableRoles { get; set; }

    public bool IsAtBaseline => CurrentRole.HierarchyLevel == 1;
}