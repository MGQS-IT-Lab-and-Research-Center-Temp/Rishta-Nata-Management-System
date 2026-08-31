using Infrastructure.DTOs.Roles;
namespace Infrastructure.DTOs;

public class RoleManagementDto
{
    public Guid MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ChandaNo { get; set; } = string.Empty;
    public IEnumerable<RoleDto> CurrentRoles { get; set; } = new List<RoleDto>();
    public IEnumerable<RoleDto> AvailableRoles { get; set; } = new List<RoleDto>();
    public bool IsAtBaseline => CurrentRoles.Count() == 1 && CurrentRoles.First().HierarchyLevel == 1;
}