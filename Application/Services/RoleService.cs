using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DTOs.Roles;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Role catalogue CRUD + dropdown search, used by RoleController.
/// </summary>
public class RoleService : IRoleService
{
    private readonly RishtanataDbContext _context;

    public RoleService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        return await _context.JamaatRoles
            .AsNoTracking()
            .OrderBy(r => r.HierarchyLevel)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                HierarchyLevel = r.HierarchyLevel
            })
            .ToListAsync();
    }

    public async Task<RoleDto?> GetRoleByIdAsync(Guid id)
    {
        return await _context.JamaatRoles
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                HierarchyLevel = r.HierarchyLevel
            })
            .FirstOrDefaultAsync();
    }

    // Dropdown search — matches by name, ordered by hierarchy
    public async Task<IEnumerable<RoleDto>> SearchRolesAsync(string? searchTerm)
    {
        var query = _context.JamaatRoles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(r => r.Name.ToLower().Contains(term));
        }

        return await query
            .OrderBy(r => r.HierarchyLevel)
            .ThenBy(r => r.Name)
            .Take(20)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                HierarchyLevel = r.HierarchyLevel
            })
            .ToListAsync();
    }

    public async Task<Role> CreateRoleAsync(Role role, string createdBy)
    {
        role.UpdatedBy = createdBy;
        _context.JamaatRoles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<Role?> UpdateRoleAsync(Guid id, Role role, string updatedBy)
    {
        var existing = await _context.JamaatRoles.FindAsync(id);
        if (existing == null) return null;

        existing.Name = role.Name;
        existing.Description = role.Description;
        existing.HierarchyLevel = role.HierarchyLevel;
        existing.UpdatedBy = updatedBy;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteRoleAsync(Guid id)
    {
        var existing = await _context.JamaatRoles.FindAsync(id);
        if (existing == null) return false;

        _context.JamaatRoles.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}