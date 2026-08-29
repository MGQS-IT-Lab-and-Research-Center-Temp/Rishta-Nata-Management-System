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
    private readonly RoleManager<ApplicationRole> _roleManager;
    public RoleAssignmentService(RishtanataDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }
    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        => await _context.JamaatRoles
            .OrderBy(r => r.HierarchyLevel)
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name!, HierarchyLevel = r.HierarchyLevel })
            .ToListAsync();

    public async Task<RoleManagementDto> GetRoleManagementAsync(Guid memberId)
    {
        var member = await _context.JamaatMembers
            .Include(m => m.MemberRoles)
                .ThenInclude(mr => mr.Role)
            .FirstOrDefaultAsync(m => m.Id == memberId)
            ?? throw new InvalidOperationException("Member not found.");

        var roleList = (await GetAllRolesAsync()).ToList();
        var currentRoleIds = member.MemberRoles.Select(mr => mr.RoleId).ToHashSet();
        var currentRoles = roleList.Where(r => currentRoleIds.Contains(r.Id)).ToList();

        if (!currentRoles.Any())
            throw new InvalidOperationException("Member has no assigned roles.");

        return new RoleManagementDto
        {
            MemberId = member.Id,
            FullName = member.FullName,
            ChandaNo = member.ChandaNo,
            CurrentRoles = currentRoles,
            AvailableRoles = roleList.Where(r => !currentRoleIds.Contains(r.Id)).ToList()
        };
    }

    public async Task<(bool Success, string? Error)> AssignRoleAsync(Guid memberId, Guid roleId, string changedBy)
    {
        var member = await _context.JamaatMembers
            .Include(m => m.MemberRoles)
            .FirstOrDefaultAsync(m => m.Id == memberId);
        if (member == null)
            return (false, "Member not found.");

        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            return (false, "Selected role does not exist.");

        if (member.MemberRoles.Any(mr => mr.RoleId == roleId))
            return (false, "Member already holds that role.");

        member.MemberRoles.Add(new JamaatMemberRole
        {
            JamaatMemberId = member.Id,
            RoleId = role.Id,
            AssignedAt = DateTime.UtcNow,
            AssignedBy = changedBy
        });

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> RemoveRoleAsync(Guid memberId, Guid roleId, string changedBy)
    {
        var member = await _context.JamaatMembers
            .Include(m => m.MemberRoles)
            .FirstOrDefaultAsync(m => m.Id == memberId);
        if (member == null)
            return (false, "Member not found.");

        var existing = member.MemberRoles.FirstOrDefault(mr => mr.RoleId == roleId);
        if (existing == null)
            return (false, "Member does not hold that role.");

        if (member.MemberRoles.Count == 1)
            return (false, "Member must retain at least one role.");

        _context.Remove(existing);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> ResetToBaseRoleAsync(Guid memberId, string changedBy)
    {
        var member = await _context.JamaatMembers
            .Include(m => m.MemberRoles)
                .ThenInclude(mr => mr.Role)
            .FirstOrDefaultAsync(m => m.Id == memberId);
        if (member == null)
            return (false, "Member not found.");

        if (member.MemberRoles.Count == 1 &&
            member.MemberRoles.First().Role.HierarchyLevel == BaselineHierarchyLevel)
            return (false, "Member is already at the Jama'at Member baseline role.");

        var baseRole = await _context.JamaatRoles
            .FirstOrDefaultAsync(r => r.HierarchyLevel == BaselineHierarchyLevel);
        if (baseRole == null)
            return (false, "Baseline Jama'at Member role is not configured.");

        // Remove every role except the baseline, then ensure baseline is present.
        var toRemove = member.MemberRoles.Where(mr => mr.RoleId != baseRole.Id).ToList();
        foreach (var mr in toRemove)
            _context.Remove(mr);

        if (!member.MemberRoles.Any(mr => mr.RoleId == baseRole.Id))
        {
            member.MemberRoles.Add(new JamaatMemberRole
            {
                JamaatMemberId = member.Id,
                RoleId = baseRole.Id,
                AssignedAt = DateTime.UtcNow,
                AssignedBy = changedBy
            });
        }

        await _context.SaveChangesAsync();
        return (true, null);
    }
}