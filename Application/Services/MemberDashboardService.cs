using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.MemberDashboard;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MemberDashboardService : IMemberDashboardService
{
    private readonly RishtanataDbContext _context;

    public MemberDashboardService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<MemberDashboardDto> GetDashboardAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        var dto = new MemberDashboardDto();

        if (string.IsNullOrWhiteSpace(membershipNo))
        {
            return dto;
        }

        var no = membershipNo.Trim();

        var member = await _context.JamaatMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ChandaNo == no, cancellationToken);

        dto.MemberName = member is null
            ? string.Empty
            : $"{member.FirstName} {member.Surname}".Trim();

        var forms = await _context.MarriageApplicationForms
            .AsNoTracking()
            .Include(x => x.MarriageApplication)
                .ThenInclude(x => x.Certificate)
            .Where(x => x.BridegroomMembershipNo == no || x.BrideMembershipNo == no)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        dto.MarriageHistory = forms
            .Select(x => new MarriageHistoryEntryDto
            {
                SpouseName = SpouseName(x, no),
                Status = ToStatusLabel(x),
                Date = MarriageDate(x)
            })
            .ToList();

        dto.CurrentSpouse = forms
            .Where(x => x.MarriageApplication?.Certificate != null ||
                        x.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
            .Select(x => new SpouseInfoDto
            {
                Name = SpouseName(x, no),
                MarriageDate = MarriageDate(x)
            })
            .OrderByDescending(x => x.MarriageDate)
            .FirstOrDefault();

        return dto;
    }

    public async Task<List<MemberApplicationDto>> GetApplicationsAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(membershipNo))
        {
            return new List<MemberApplicationDto>();
        }

        var no = membershipNo.Trim();

        return await _context.MarriageApplicationForms
            .AsNoTracking()
            .Include(x => x.MarriageApplication)
            .Where(x => x.BridegroomMembershipNo == no || x.BrideMembershipNo == no)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MemberApplicationDto
            {
                Id = x.Id,
                ReferenceNumber = x.ReferenceNumber,
                SpouseName = x.BridegroomMembershipNo == no ? x.BrideName : x.BridegroomName,
                Role = x.BridegroomMembershipNo == no ? "Groom" : "Bride",
                Status = x.MarriageApplication!.Status.ToString(),
                SubmittedDate = x.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<MemberProfileDto?> GetProfileAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(membershipNo))
        {
            return null;
        }

        var member = await _context.JamaatMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ChandaNo == membershipNo.Trim(), cancellationToken);

        if (member is null)
        {
            return null;
        }

        return new MemberProfileDto
        {
            Id = member.Id,
            Surname = member.Surname,
            FirstName = member.FirstName,
            MiddleName = member.MiddleName,
            Title = member.Title,
            FullName = $"{member.FirstName} {member.Surname}".Trim(),
            Email = member.Email,
            ChandaNo = member.ChandaNo,
            WasiyatNo = member.WasiyatNo,
            AuxillaryBodyName = member.AuxillaryBodyName,
            DateOfBirth = member.DateOfBirth,
            PhoneNo = member.PhoneNo,
            JamaatName = member.JamaatName,
            CircuitName = member.CircuitName,
            Sex = member.Sex,
            MaritalStatus = member.MaritalStatus,
            Address = member.Address,
            Nationality = member.Nationality,
            RoleName = string.IsNullOrWhiteSpace(member.Roles) ? null : member.Roles
        };
    }

    private static string SpouseName(MarriageApplicationForm form, string membershipNo) =>
        string.Equals(form.BridegroomMembershipNo, membershipNo, StringComparison.OrdinalIgnoreCase)
            ? form.BrideName
            : form.BridegroomName;

    private static DateTime MarriageDate(MarriageApplicationForm form) =>
        form.ApprovedDateOfNikah
        ?? form.MarriageApplication?.Certificate?.NikahDate
        ?? form.CreatedAt;

    private static string ToStatusLabel(MarriageApplicationForm form)
    {
        if (form.MarriageApplication?.Certificate != null ||
            form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
        {
            return "Married";
        }

        return form.MarriageApplication?.Status switch
        {
            ApplicationStatus.ApplicationRejected => "Rejected",
            ApplicationStatus.AwaitingMoreInformation => "Awaiting More Information",
            ApplicationStatus.ApplicationPending => "Pending",
            ApplicationStatus.Submitted => "Submitted",
            ApplicationStatus.Draft => "Draft",
            _ => "Pending"
        };
    }
}
