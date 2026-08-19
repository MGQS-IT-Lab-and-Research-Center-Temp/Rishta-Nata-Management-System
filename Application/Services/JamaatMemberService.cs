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
        var existingMember = await _context.JamaatMembers
            .FirstOrDefaultAsync(x => x.chandaNo == member.chandaNo);

        // FIRST LOGIN
        if (existingMember == null)
        {
            var newMember = new JamaatMember
            {
                surname = member.surname,
                firstName = member.firstName,
                email = member.email,
                chandaNo = member.chandaNo,
                wasiyatNo = member.wasiyatNo,
                title = member.title,
                auxillaryBodyName = member.auxillaryBodyName,
                middleName = member.middleName,
                maidenName = member.maidenName,
                dateOfBirth = member.dateOfBirth,
                phoneNo = member.phoneNo,
                jamaatName = member.jamaatName,
                circuitName = member.circuitName,
                sex = member.sex,
                maritalStatus = member.maritalStatus,
                address = member.address,
                nextOfKinPhoneNo = member.nextOfKinPhoneNo,
                nextOfKinName = member.nextOfKinName,
                nextOfKinAddress = member.nextOfKinAddress,
                nationality = member.nationality,

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

        existingMember.surname = member.surname;
        existingMember.firstName = member.firstName;
        existingMember.email = member.email;
        existingMember.wasiyatNo = member.wasiyatNo;
        existingMember.title = member.title;
        existingMember.auxillaryBodyName = member.auxillaryBodyName;
        existingMember.middleName = member.middleName;
        existingMember.maidenName = member.maidenName;
        existingMember.phoneNo = member.phoneNo;
        existingMember.jamaatName = member.jamaatName;
        existingMember.circuitName = member.circuitName;
        existingMember.maritalStatus = member.maritalStatus;
        existingMember.address = member.address;
        existingMember.nextOfKinPhoneNo = member.nextOfKinPhoneNo;
        existingMember.nextOfKinName = member.nextOfKinName;
        existingMember.nextOfKinAddress = member.nextOfKinAddress;
        existingMember.nationality = member.nationality;

        // Don't change:
        // existingMember.Id
        // existingMember.chandaNo
        // existingMember.Password
        // existingMember.CreatedAt
        // existingMember.CreatedBy
        // existingMember.ResetToken
        // existingMember.ResetTokenExpiry
        // existingMember.IsSystemDefault

        existingMember.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existingMember;
    }
}