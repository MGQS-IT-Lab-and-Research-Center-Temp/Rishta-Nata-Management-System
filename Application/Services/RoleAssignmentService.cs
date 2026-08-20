
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Roles;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class RoleAssignmentService : IRoleAssignmentService
{
    private const int BaselineHierarchyLevel = 1; // Jama'at Member

    private readonly RishtanataDbContext _context;
    private readonly RoleManager<RoleDto> _roleManager;

    public RoleAssignmentService(RishtanataDbContext context, RoleManager<RoleDto> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        => await _roleManager.Roles
            .OrderBy(r => r.HierarchyLevel)
            .Select(r => new RoleDto {Id = r.Id, Name = r.Name!, HierarchyLevel = r.HierarchyLevel })
            .ToListAsync();
    public async Task<RoleManagementDto> GetRoleManagementAsync(Guid memberId)
    {
        var member = await _context.JamaatMembers
            .Include(m => m.Role)
            .FirstOrDefaultAsync(m => m.Id == memberId)
            ?? throw new InvalidOperationException("Member not found.");

        var roleList = (await GetAllRolesAsync()).ToList();

        var currentRole = roleList.FirstOrDefault(r => r.Id == member.RoleId)
            ?? throw new InvalidOperationException("Member's assigned role no longer exists.");

        return new RoleManagementDto
        {
            MemberId = member.Id,
            FullName = member.FullName,
            ChandaNo = member.chandaNo,
            CurrentRole = currentRole,
            AvailableRoles = roleList.Where(r => r.Id != member.RoleId).ToList()
        };
    }

    public async Task<(bool Success, string? Error)> AssignRoleAsync(Guid memberId, Guid roleId, string changedBy)
    {
        var member = await _context.JamaatMembers.FirstOrDefaultAsync(m => m.Id == memberId);
        if (member == null)
            return (false, "Member not found.");

        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            return (false, "Selected role does not exist.");

        if (member.RoleId == roleId)
            return (false, "Member already holds that role.");

        member.RoleId = role.Id;
        member.Role.UpdatedBy = changedBy; 
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> ResetToBaseRoleAsync(Guid memberId, string changedBy)
    {
        var member = await _context.JamaatMembers
            .Include(m => m.Role)
            .FirstOrDefaultAsync(m => m.Id == memberId);

        if (member == null)
            return (false, "Member not found.");

        if (member.Role.HierarchyLevel == BaselineHierarchyLevel)
            return (false, "Member is already at the Jama'at Member baseline role.");

        var baseRole = await _roleManager.Roles
            .FirstOrDefaultAsync(r => r.HierarchyLevel == BaselineHierarchyLevel);

        if (baseRole == null)
            return (false, "Baseline Jama'at Member role is not configured.");

        member.RoleId = baseRole.Id;
        member.Role.UpdatedBy = changedBy;

        await _context.SaveChangesAsync();
        return (true, null);
    }
}