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

    public async Task<JamaatMember> CreateOrUpdateAsync(JamaatMember member)
    {
        var existingMember = await _context.JamaatMembers.FirstOrDefaultAsync(x => x.ChandaNo == member.ChandaNo);

        // FIRST LOGIN
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

            // These should normally be handled by your application,
            // not copied blindly from the gateway.
            RoleId = member.RoleId,
                IsSystemDefault = false,

                CreatedAt = DateTime.UtcNow
            };

            _context.JamaatMembers.Add(newMember);

            await _context.SaveChangesAsync();

            return newMember;
        }

        // SUBSEQUENT LOGINS
        // Update only properties that can change in real time.

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

        // Don't change:
        // existingMember.Id
        // existingMember.chandaNo
        // existingMember.Password
        // existingMember.CreatedAt
        // existingMember.CreatedBy
        // existingMember.ResetToken
        // existingMember.ResetTokenExpiry
        // existingMember.IsSystemDefault

        await _context.SaveChangesAsync();

        return existingMember;
    }
}