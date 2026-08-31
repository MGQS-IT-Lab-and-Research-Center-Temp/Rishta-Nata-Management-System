using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class JamaatMemberService : IJamaatMemberService
{
    private const int BaselineHierarchyLevel = 1; // Jama'at Member

    private readonly RishtanataDbContext _context;

    public JamaatMemberService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<JamaatMember> CreateOrUpdateAsync(JamaatMember member)
    {
        var existingMember = await _context.JamaatMembers
            .Include(m => m.MemberRoles)
                .ThenInclude(mr => mr.Role)
            .FirstOrDefaultAsync(x => x.ChandaNo == member.ChandaNo);

        if (existingMember == null)
        {
            var resolvedRoleId = await ResolveRoleIdAsync(member);

            var newMember = new JamaatMember
            {
                Surname = member.Surname,
                FirstName = member.FirstName,
                Email = member.Email,
                ChandaNo = member.ChandaNo,
                WasiyatNo = member.WasiyatNo,
                Title = member.Title,
                AuxillaryBodyName = member.AuxillaryBodyName,
                MiddleName = member.MiddleName,
                MaidenName = member.MaidenName,
                DateOfBirth = member.DateOfBirth,
                PhoneNo = member.PhoneNo,
                JamaatName = member.JamaatName,
                CircuitName = member.CircuitName,
                Sex = member.Sex,
                MaritalStatus = member.MaritalStatus,
                Address = member.Address,
                NextOfKinPhoneNo = member.NextOfKinPhoneNo,
                NextOfKinName = member.NextOfKinName,
                NextOfKinAddress = member.NextOfKinAddress,
                Nationality = member.Nationality,
                IsSystemDefault = false,
                CreatedAt = DateTime.UtcNow
            };

            if (resolvedRoleId is Guid roleId)
            {
                newMember.MemberRoles.Add(new JamaatMemberRole
                {
                    RoleId = roleId,
                    AssignedAt = DateTime.UtcNow,
                    AssignedBy = "system:first-login-default"
                });
            }

            _context.JamaatMembers.Add(newMember);
            await _context.SaveChangesAsync();

            return await _context.JamaatMembers
                .Include(m => m.MemberRoles)
                    .ThenInclude(mr => mr.Role)
                .FirstAsync(m => m.Id == newMember.Id);
        }

        existingMember.Surname = member.Surname;
        existingMember.FirstName = member.FirstName;
        existingMember.Email = member.Email;
        existingMember.WasiyatNo = member.WasiyatNo;
        existingMember.Title = member.Title;
        existingMember.AuxillaryBodyName = member.AuxillaryBodyName;
        existingMember.MiddleName = member.MiddleName;
        existingMember.MaidenName = member.MaidenName;
        existingMember.PhoneNo = member.PhoneNo;
        existingMember.JamaatName = member.JamaatName;
        existingMember.CircuitName = member.CircuitName;
        existingMember.MaritalStatus = member.MaritalStatus;
        existingMember.Address = member.Address;
        existingMember.NextOfKinPhoneNo = member.NextOfKinPhoneNo;
        existingMember.NextOfKinName = member.NextOfKinName;
        existingMember.NextOfKinAddress = member.NextOfKinAddress;
        existingMember.Nationality = member.Nationality;
        existingMember.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existingMember;
    }

    private async Task<Guid?> ResolveRoleIdAsync(JamaatMember member)
    {
        var roleName = member.MemberRoles?.FirstOrDefault()?.Role?.Name?.Trim();
        if (!string.IsNullOrWhiteSpace(roleName))
        {
            var byName = await _context.JamaatRoles
                .FirstOrDefaultAsync(r => r.Name == roleName);
            if (byName is not null)
            {
                return byName.Id;
            }
        }
        var baseline = await _context.JamaatRoles
            .FirstOrDefaultAsync(r => r.HierarchyLevel == BaselineHierarchyLevel);
        return baseline?.Id;
    }
}