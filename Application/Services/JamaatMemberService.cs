using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class JamaatMemberService : IJamaatMemberService
{
    private static readonly TimeSpan DefaultFreshnessWindow = TimeSpan.FromHours(24);
    private readonly RishtanataDbContext _context;

    public JamaatMemberService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<JamaatMember> CreateOrUpdateAsync(JamaatMember member)
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
                Roles = member.Roles,
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
        existingMember.Roles = member.Roles;
        existingMember.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existingMember;
    }

    public async Task<JamaatMember?> GetByChandaNoAsync(
        string chandaNo, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(chandaNo))
        {
            return null;
        }

        return await _context.JamaatMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ChandaNo == chandaNo, cancellationToken);
    }

    public bool IsProfileFresh(JamaatMember member, TimeSpan? maxAge = null)
    {
        ArgumentNullException.ThrowIfNull(member);

        maxAge ??= DefaultFreshnessWindow;

        var stamp = member.ModifiedAt != default ? member.ModifiedAt : member.CreatedAt;

        return DateTime.UtcNow - stamp < maxAge.Value;
    }

    public async Task<JamaatMember> UpdateRolesAsync(
        string chandaNo, IEnumerable<string> roles, CancellationToken cancellationToken = default)
    {
        var member = await _context.JamaatMembers
            .FirstOrDefaultAsync(x => x.ChandaNo == chandaNo, cancellationToken)
            ?? throw new InvalidOperationException(
                $"Member {chandaNo} does not exist locally; cannot update roles.");

        member.Roles = string.Join(",", roles ?? Array.Empty<string>());
        member.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return member;
    }
}
