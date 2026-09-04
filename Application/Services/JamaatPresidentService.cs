using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.JamaatPresidentDashboardDto;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Jamaat (branch) President dashboard plus per-application review actions
/// (approve / reject / request more info).
/// Cleanup: namespace was Application.Interfaces; moved to Application.Services
/// so the namespace matches the folder where the implementation lives.
/// </summary>
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
        // Cleanup: treated AwaitingMoreInformation as pending (a form sent back
        // for corrections still awaits the next review step on the dashboard).
        var pendingStatuses = new[]
        {
            ApplicationStatus.ApplicationPending,
            ApplicationStatus.AwaitingMoreInformation
        };

        var jamaatMember = currentUserId.HasValue
            ? await _context.JamaatMembers
                .FirstOrDefaultAsync(x => x.Id == currentUserId.Value)
            : null;

        if (jamaatMember == null)
        {
            throw new InvalidOperationException(
                currentUserId.HasValue
                    ? $"No Jama'at member was found for the current user ID '{currentUserId.Value}'."
                    : "Unable to load the Jama'at member because no current user ID was provided.");
        }

        var pendingApplications = await _context.FormApplications
            .Where(x => pendingStatuses.Contains(x.Status))
            .Include(x => x.MarriageApplicationForm)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var totalApplications = await _context.FormApplications.CountAsync();

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var reviewedToday = await _context.AuditLogs
            .CountAsync(x =>
                x.CreatedAt >= today &&
                x.CreatedAt < tomorrow &&
                (
                    x.Action == "Approved Nikah Application" ||
                    x.Action == "Rejected Nikah Application" ||
                    x.Action == "Requested More Information"
                ));

        var recentActivities = await _context.AuditLogs
            .OrderByDescending(x => x.CreatedAt)
            .Take(10)
            .Select(x => new RecentActivityDto
            {
                ApplicationNumber = x.EntityName,
                Description = x.Action,
                Date = x.CreatedAt
            })
            .ToListAsync();

        return new JamaatPresidentDashboardDto
        {
            PresidentName = presidentDisplayName ?? "Jama'at President",
            JamaatName = jamaatMember.JamaatName ?? "Jama'at",
            CircuitName = jamaatMember.CircuitName ?? "Circuit",
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
            .Include(r => r.MarriageApplication)
                .ThenInclude(r => r.MarriageApplicationForm)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            throw new KeyNotFoundException(
                $"No review was found with ID '{id}'.");
        }

        var form = review.MarriageApplication?.MarriageApplicationForm;

        if (form == null)
        {
            throw new InvalidOperationException(
                $"Review '{id}' does not have an associated marriage application form.");
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
            NationalRishtanataSecretarySignatureDate =
                form.NationalRishtanataSecretarySignatureDate,

            ApprovedDateOfNikah = form.ApprovedDateOfNikah,
            NationalAmirOrMissionarySignatureDate =
                form.NationalAmirOrMissionarySignatureDate
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
        // Cleanup: was ApplicationPending (a no-op). The distinct
        // AwaitingMoreInformation status now records the request for corrections.
        return await ChangeStatusAsync(
            id,
            currentUserId,
            ApplicationStatus.AwaitingMoreInformation,
            "Requested More Information",
            "");
    }

    private async Task<bool> ChangeStatusAsync(
        Guid id,
        Guid? currentUserId,
        ApplicationStatus newStatus,
        string actionLabel,
        string extraDetail)
    {
        var application = await _context.FormApplications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return false;
        }

        // Cleanup: allow approve/reject/request-info from both pending states so a
        // form that was sent back (AwaitingMoreInformation) isn't orphaned.
        if (application.Status != ApplicationStatus.ApplicationPending &&
            application.Status != ApplicationStatus.AwaitingMoreInformation)
        {
            return false;
        }

        application.Status = newStatus;
        application.ModifiedAt = DateTime.UtcNow;
        application.ModifiedBy = currentUserId;

        var reference =
            application.MarriageApplicationForm?.ReferenceNumber
            ?? application.Id.ToString();

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = currentUserId ?? Guid.Empty,
            Action = actionLabel,
            EntityName = "MarriageApplication",
            RecordId = application.Id,
            CreatedAt = DateTime.UtcNow,
            ChangeDetails =
                $"Jama'at President {actionLabel.ToLower()} application {reference}. {extraDetail}"
                    .Trim()
        };

        _context.AuditLogs.Add(auditLog);

        await _context.SaveChangesAsync();

        return true;
    }
}
