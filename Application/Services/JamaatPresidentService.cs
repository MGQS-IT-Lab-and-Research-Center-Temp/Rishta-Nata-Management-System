using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.JamaatPresidentDashboardDto;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public class JamaatPresidentService : IJamaatPresidentService
{
    private readonly RishtanataDbContext _context;

    public JamaatPresidentService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<JamaatPresidentDashboardDto> GetDashboardAsync(
    string? presidentDisplayName,
    Guid? currentUserId)
    {
        var pendingStatus = ApplicationStatus.ApplicationPending;

        var jamaatMember = currentUserId.HasValue
    ? await _context.JamaatMembers
        .FirstOrDefaultAsync(x => x.Id == currentUserId.Value)
    : null;

        var pendingApplications = await _context.FormApplications
            .Where(x => x.Status == pendingStatus)
            .Include(x => x.MarriageApplicationForm)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var totalApplications = await _context.FormApplications.CountAsync();

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var reviewedToday = await _context.AuditLogs
            .CountAsync(x =>
                x.Timestamp >= today &&
                x.Timestamp < tomorrow &&
                (
                    x.Action == "Approved Nikah Application" ||
                    x.Action == "Rejected Nikah Application" ||
                    x.Action == "Requested More Information"
                ));

        var recentActivities = await _context.AuditLogs
            .OrderByDescending(x => x.Timestamp)
            .Take(10)
            .Select(x => new RecentActivityDto
            {
                ApplicationNumber = x.EntityName,
                Description = x.Action,
                Date = x.Timestamp
            })
            .ToListAsync();

        return new JamaatPresidentDashboardDto
        {
            PresidentName = presidentDisplayName ?? "Jama'at President",
            JamaatName = jamaatMember?.jamaatName ?? "Jama'at",
            CircuitName = jamaatMember?.circuitName ?? "Circuit",
            PendingNikahReviews = pendingApplications.Count,
            ReviewedToday = reviewedToday,
            TotalNikahApplications = totalApplications,
            PendingApplications = pendingApplications
                .Select(x => new NikahApplicationDto
                {
                    Id = x.Id,
                    ReferenceNumber = x.MarriageApplicationForm?.ReferenceNumber ?? "N/A",
                    GroomName = x.MarriageApplicationForm?.BridegroomName ?? "Not provided",
                    BrideName = x.MarriageApplicationForm?.BrideName ?? "Not provided",
                    JamaatName = x.MarriageApplicationForm?.Venue ?? "Not provided",
                    SubmittedDate = x.CreatedAt,
                    Status = x.Status.ToString()
                })
                .ToList(),
            RecentActivities = recentActivities
        };
    }

    public async Task<JamaatPresidentReviewDto?> GetReviewByIdAsync(Guid id)
    {
        var review = await _context.Reviews
            .Include(r => r.MarriageApplicationForm)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return null;
        }

        var form = review.MarriageApplicationForm;

        if (form == null)
        {
            return null;
        }

        return new JamaatPresidentReviewDto
        {
            Id = review.Id,
            ReferenceNumber = form.ReferenceNumber,
            Status = review.Status,
            SubmittedDate = review.ReviewedAt,

            ProposedNikahDate = form.ProposedNikahDate,
            Venue = form.Venue,

            BrideMembershipNo = form.BrideMembershipNo,
            BrideName = form.BrideName,
            BrideDateOfBirth = form.BrideDateOfBirth,
            BrideResidentOf = form.BrideResidentOf,
            BrideGenotype = form.BrideGenotype,
            BrideBloodGroup = form.BrideBloodGroup,
            BrideMaritalStatus = form.BrideMaritalStatus,
            BrideProposedDowerAmount = form.BrideProposedDowerAmount,
            BrideDowerAmountReceivedInCash = form.BrideDowerAmountReceivedInCash,
            BrideSignatureTel = form.BrideSignatureTel,

            BridegroomMembershipNo = form.BridegroomMembershipNo,
            BridegroomName = form.BridegroomName,
            BridegroomDateOfBirth = form.BridegroomDateOfBirth,
            BridegroomResidentOf = form.BridegroomResidentOf,
            BridegroomGenotype = form.BridegroomGenotype,
            BridegroomBloodGroup = form.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash = form.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = form.BridegroomDowerAmountToBePaid,
            IsFirstNikah = form.IsFirstNikah,
            IsSecondThirdOrFourthNikah = form.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = form.FormerWifeIsDead,
            HasDivorcedFormerWife = form.HasDivorcedFormerWife,
            FormerWifeIsPresent = form.FormerWifeIsPresent,
            FormerWifeObtainedKhula = form.FormerWifeObtainedKhula,
            BridegroomSignatureTel = form.BridegroomSignatureTel,

            BrideFatherName = form.BrideFatherName,
            BridegroomFatherName = form.BridegroomFatherName,

            GuardianName = form.GuardianName,
            GuardianRelationToBride = form.GuardianRelationToBride,
            GuardianAddress = form.GuardianAddress,
            GuardianTel = form.GuardianTel,
            GuardianSignatureDate = form.GuardianSignatureDate,

            RepresentativeName = form.RepresentativeName,
            RepresentativeAddress = form.RepresentativeAddress,
            RepresentativeActingFor = form.RepresentativeActingFor,
            RepresentativeSignatureDate = form.RepresentativeSignatureDate,

            WitnessOneName = form.WitnessOneName,
            WitnessOneAddress = form.WitnessOneAddress,
            WitnessOneTel = form.WitnessOneTel,
            WitnessOneSignatureDate = form.WitnessOneSignatureDate,

            WitnessTwoName = form.WitnessTwoName,
            WitnessTwoAddress = form.WitnessTwoAddress,
            WitnessTwoTel = form.WitnessTwoTel,
            WitnessTwoSignatureDate = form.WitnessTwoSignatureDate,

            OfficiatingImamName = form.OfficiatingImamName,
            OfficiatingImamAddressJamaat = form.OfficiatingImamAddressJamaat,
            OfficiatingImamSignatureDate = form.OfficiatingImamSignatureDate,

            JamaatPresidentName = form.JamaatPresidentName,
            JamaatPresidentSignatureDate = form.JamaatPresidentSignatureDate,

            NationalRishtanataSecretaryName = form.NationalRishtanataSecretaryName,
            NationalRishtanataSecretarySignatureDate = form.NationalRishtanataSecretarySignatureDate,

            ApprovedDateOfNikah = form.ApprovedDateOfNikah,
            NationalAmirOrMissionarySignatureDate = form.NationalAmirOrMissionarySignatureDate
        };
    }
    public async Task<bool> ApproveAsync(Guid id, Guid? currentUserId)
    {
        return await ChangeStatusAsync(
            id,
            currentUserId,
            ApplicationStatus.ApplicationApproved,
            "Approved Nikah Application",
            "and forwarded it for National Rishtanata Secretary review.");
    }

    public async Task<bool> RejectAsync(Guid id, Guid? currentUserId)
    {
        return await ChangeStatusAsync(
            id,
            currentUserId,
            ApplicationStatus.ApplicationRejected,
            "Rejected Nikah Application",
            "");
    }

    public async Task<bool> RequestMoreInformationAsync(Guid id, Guid? currentUserId)
    {
        return await ChangeStatusAsync(
            id,
            currentUserId,
            ApplicationStatus.ApplicationPending,
            "Requested More Information",
            "");
    }

    private async Task<bool> ChangeStatusAsync(Guid id, Guid? currentUserId, ApplicationStatus newStatus, string actionLabel, string extraDetail)
    {
        var application = await _context.FormApplications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return false;
        }

        if (application.Status != ApplicationStatus.ApplicationPending)
        {
            return false;
        }

        application.Status = newStatus;
        application.ModifiedAt = DateTime.UtcNow;
        application.ModifiedBy = currentUserId;

        var reference = application.MarriageApplicationForm?.ReferenceNumber ?? application.Id.ToString();

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = currentUserId ?? Guid.Empty,
            Action = actionLabel,
            EntityName = "MarriageApplication",
            RecordId = application.Id,
            Timestamp = DateTime.UtcNow,
            ChangeDetails = $"Jama'at President {actionLabel.ToLower()} application {reference}. {extraDetail}".Trim()
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();

        return true;
    }
}
