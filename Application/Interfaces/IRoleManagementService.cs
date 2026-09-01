using Infrastructure.DTOs;
using Infrastructure.DTOs.Roles;

namespace Application.Interfaces;

public interface IRoleAssignmentService

{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleManagementDto> GetRoleManagementAsync(Guid memberId);
    Task<(bool Success, string? Error)> AssignRoleAsync(Guid memberId, Guid roleId, string changedBy);
    Task<(bool Success, string? Error)> RemoveRoleAsync(Guid memberId, Guid roleId, string changedBy);
    Task<(bool Success, string? Error)> ResetToBaseRoleAsync(Guid memberId, string changedBy);
}