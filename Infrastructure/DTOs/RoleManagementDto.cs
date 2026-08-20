using Infrastructure.DTOs.Roles;

namespace Infrastructure.DTOs;

public class RoleManagementDto
{
    public Guid MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ChandaNo { get; set; } = string.Empty;

    public RoleDto CurrentRole { get; set; } = default!;
    public List<RoleDto> AvailableRoles { get; set; } = new();

    public bool IsAtBaseline => CurrentRole.HierarchyLevel == 1;
}


