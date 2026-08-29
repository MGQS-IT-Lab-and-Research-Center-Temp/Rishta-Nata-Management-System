// Application/Roles/IRoleService.cs
using Domain.Entities;
using Infrastructure.DTOs.Roles;

namespace Application.Roles;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(Guid id);

    // Powers the dashboard dropdown search
    Task<IEnumerable<RoleDto>> SearchRolesAsync(string? searchTerm);

    Task<Role> CreateRoleAsync(Role role, string createdBy);
    Task<Role?> UpdateRoleAsync(Guid id, Role role, string updatedBy);
    Task<bool> DeleteRoleAsync(Guid id);
}