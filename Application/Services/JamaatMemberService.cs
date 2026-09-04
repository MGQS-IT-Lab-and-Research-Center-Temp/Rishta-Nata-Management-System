using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class JamaatMemberService : IJamaatMemberService
{
    private readonly RishtanataDbContext _context;

    public JamaatMemberService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<JamaatMember> CreateOrUpdateAsync(
        JamaatMember member)
    {
        var existingMember = await _context.JamaatMembers
            .FirstOrDefaultAsync(x => x.ChandaNo == member.ChandaNo);

        if (existingMember == null)
        {
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
                DateOfBirth = member.DateOfBirth,
                PhoneNo = member.PhoneNo,
                JamaatName = member.JamaatName,
                CircuitName = member.CircuitName,
                Sex = member.Sex,
                MaritalStatus = member.MaritalStatus,
                Address = member.Address,
                Nationality = member.Nationality,
                CreatedAt = DateTime.UtcNow
            };

            _context.JamaatMembers.Add(newMember);

            await _context.SaveChangesAsync();

            return newMember;
        }

        existingMember.Surname = member.Surname;
        existingMember.FirstName = member.FirstName;
        existingMember.Email = member.Email;
        existingMember.WasiyatNo = member.WasiyatNo;
        existingMember.Title = member.Title;
        existingMember.AuxillaryBodyName = member.AuxillaryBodyName;
        existingMember.MiddleName = member.MiddleName;
        existingMember.PhoneNo = member.PhoneNo;
        existingMember.JamaatName = member.JamaatName;
        existingMember.CircuitName = member.CircuitName;
        existingMember.MaritalStatus = member.MaritalStatus;
        existingMember.Address = member.Address;
        existingMember.Nationality = member.Nationality;
        existingMember.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existingMember;
    }
}
