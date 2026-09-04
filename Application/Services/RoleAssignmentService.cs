//using Application.Interfaces;
//using Domain.Entities;
//using Infrastructure.DTOs;
//using Infrastructure.DTOs.Roles;
//using Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;
//namespace Application.Services;

//// Cleanup: file renamed from RoleManagementService.cs to match the class it
//// contains (RoleAssignmentService). The unfinished logic was also completed —
//// the service previously had a throwing constructor, a stale ASP.NET Identity
//// dependency, a NotImplementedException in RemoveRoleAsync, and commented-out
//// writes that meant AssignRoleAsync / ResetToBaseRoleAsync persisted nothing.
//public class RoleAssignmentService : IRoleAssignmentService
//{
//    private const int BaselineHierarchyLevel = 1; // Jama'at Member
//    private readonly RishtanataDbContext _context;

//    // Single working constructor. The old overloads were: (1) one that threw
//    // NotImplementedException — the one DI actually resolved, so the service
//    // crashed on construction; and (2) one that took a stale
//    // RoleManager<ApplicationRole> (ASP.NET Identity was removed). Both gone.
//    public RoleAssignmentService(RishtanataDbContext context)
//    {
//        _context = context;
//    }

//    //public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
//    //    => await _context.JamaatRoles
//    //        .OrderBy(r => r.HierarchyLevel)
//    //        .Select(r => new RoleDto { Id = r.Id, Name = r.Name!, HierarchyLevel = r.HierarchyLevel })
//    //        .ToListAsync();

//    //public async Task<RoleManagementDto> GetRoleManagementAsync(Guid memberId)
//    //{
//    //    var member = await _context.JamaatMembers
//    //        .Include(m => m.MemberRoles)
//    //            .ThenInclude(mr => mr.Role)
//    //        .FirstOrDefaultAsync(m => m.Id == memberId)
//    //        ?? throw new InvalidOperationException("Member not found.");

//    //    var roleList = (await GetAllRolesAsync()).ToList();
//    //    var currentRoleIds = member.MemberRoles.Select(mr => mr.RoleId).ToHashSet();
//    //    var currentRoles = roleList.Where(r => currentRoleIds.Contains(r.Id)).ToList();

//    //    if (!currentRoles.Any())
//    //        throw new InvalidOperationException("Member has no assigned roles.");

//    //    return new RoleManagementDto
//    //    {
//    //        MemberId = member.Id,
//    //        FullName = member.FullName,
//    //        ChandaNo = member.ChandaNo,
//    //        CurrentRoles = currentRoles,
//    //        AvailableRoles = roleList.Where(r => !currentRoleIds.Contains(r.Id)).ToList()
//    //    };
//    //}

//    //public async Task<(bool Success, string? Error)> AssignRoleAsync(Guid memberId, Guid roleId, string changedBy)
//    //{
//    //    var member = await _context.JamaatMembers
//    //        .Include(m => m.MemberRoles)
//    //        .FirstOrDefaultAsync(m => m.Id == memberId);
//    //    if (member == null)
//    //        return (false, "Member not found.");

//    //    var role = await _context.JamaatRoles.FirstOrDefaultAsync(r => r.Id == roleId);
//    //    if (role == null)
//    //        return (false, "Selected role does not exist.");

//    //    if (member.MemberRoles.Any(mr => mr.RoleId == roleId))
//    //        return (false, "Member already holds that role.");

//    //    // A member can hold several roles at once, so assigning one means adding
//    //    // a join row (JamaatMemberRole) to the member's MemberRoles collection —
//    //    // the same pattern JamaatMemberService uses when provisioning a member at
//    //    // first login. The old code ran the guards but never added this row.
//    //    member.MemberRoles.Add(new JamaatMemberRole
//    //    {
//    //        RoleId = role.Id,
//    //        AssignedAt = DateTime.UtcNow,
//    //        AssignedBy = changedBy
//    //    });

//    //    member.ModifiedAt = DateTime.UtcNow;
//    //    await _context.SaveChangesAsync();
//    //    return (true, null);
//    //}

//    public async Task<(bool Success, string? Error)> RemoveRoleAsync(Guid memberId, Guid roleId, string changedBy)
//    {
//        var member = await _context.JamaatMembers
//            .Include(m => m.MemberRoles)
//            .FirstOrDefaultAsync(m => m.Id == memberId);
//        if (member == null)
//            return (false, "Member not found.");

//        var assignment = member.MemberRoles.FirstOrDefault(mr => mr.RoleId == roleId);
//        if (assignment is null)
//            return (false, "Member does not currently hold that role.");

//        // Removing a role = deleting just its join row; all other roles remain.
//        // (Previously threw NotImplementedException.)
//        member.MemberRoles.Remove(assignment);
//        member.ModifiedAt = DateTime.UtcNow;

//        await _context.SaveChangesAsync();
//        return (true, null);
//    }

//    public async Task<(bool Success, string? Error)> ResetToBaseRoleAsync(Guid memberId, string changedBy)
//    {
//        var member = await _context.JamaatMembers
//            .Include(m => m.MemberRoles)
//                .ThenInclude(mr => mr.Role)
//            .FirstOrDefaultAsync(m => m.Id == memberId);
//        if (member == null)
//            return (false, "Member not found.");

//        if (member.MemberRoles.Count == 1 &&
//            member.MemberRoles.First().Role.HierarchyLevel == BaselineHierarchyLevel)
//            return (false, "Member is already at the Jama'at Member baseline role.");

//        var baseRole = await _context.JamaatRoles
//            .FirstOrDefaultAsync(r => r.HierarchyLevel == BaselineHierarchyLevel);
//        if (baseRole == null)
//            return (false, "Baseline Jama'at Member role is not configured.");

//        // Reset = collapse to the baseline role only: drop every current join row
//        // and re-add the baseline (fresh AssignedAt/AssignedBy so the audit trail
//        // stays clean). The old code validated the guards but never touched the
//        // roles collection.
//        _context.Set<JamaatMemberRole>().RemoveRange(member.MemberRoles);
//        member.MemberRoles.Clear();

//        member.MemberRoles.Add(new JamaatMemberRole
//        {
//            RoleId = baseRole.Id,
//            AssignedAt = DateTime.UtcNow,
//            AssignedBy = changedBy
//        });

//        member.ModifiedAt = DateTime.UtcNow;

//        await _context.SaveChangesAsync();
//        return (true, null);
//    }
//}