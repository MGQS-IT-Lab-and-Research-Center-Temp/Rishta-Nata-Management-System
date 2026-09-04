using Domain.Entities;
using Infrastructure.DTOs.Roles;

namespace Application.Interfaces;

/// <summary>
/// Role catalogue — CRUD plus a dropdown search over Jamaat roles.
/// Cleanup: this interface used to live in the non-existent folder namespace
/// Application.Roles; moved to Application.Interfaces (all interfaces live here).
/// </summary>
public interface IRoleService
{
    //Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    //Task<RoleDto?> GetRoleByIdAsync(Guid id);

    //// Powers the dashboard dropdown search
    //Task<IEnumerable<RoleDto>> SearchRolesAsync(string? searchTerm);

    //Task<Role> CreateRoleAsync(Role role, string createdBy);
    //Task<Role?> UpdateRoleAsync(Guid id, Role role, string updatedBy);
    //Task<bool> DeleteRoleAsync(Guid id);
}