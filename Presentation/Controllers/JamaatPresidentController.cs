using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.ViewModels;
using System.Security.Claims;

namespace Presentation.Controllers;

// TODO: Restore [Authorize(Roles = "JamaatPresident")] once authentication is built.
// Temporarily removed to allow testing this dashboard without a working login flow.
public class JamaatPresidentController : Controller
{
    private readonly RishtanataDbContext _context;

    public JamaatPresidentController(RishtanataDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // DASHBOARD
    // ============================================================

    public async Task<IActionResult> Dashboard()
    {
        var pendingStatus = ApplicationStatus.ApplicationPending;

        var pendingApplications = await _context.Applications
            .Where(x => x.Status == pendingStatus)
            .Include(x => x.MarriageApplicationForm)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var totalApplications =
            await _context.ApplicationS.CountAsync();

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var currentUserId = GetCurrentUserId();

        var reviewedToday = 0;

        if (currentUserId.HasValue)
        {
            reviewedToday = await _context.AuditLogs
                .CountAsync(x =>
                    x.UserId == currentUserId.Value &&
                    x.Timestamp >= today &&
                    x.Timestamp < tomorrow &&
                    (
                        x.Action == "Approved Nikah Application" ||
                        x.Action == "Rejected Nikah Application" ||
                        x.Action == "Requested More Information"
                    ));
        }

        var recentActivities = await _context.AuditLogs
            .OrderByDescending(x => x.Timestamp)
            .Take(10)
            .Select(x => new RecentActivityViewModel
            {
                ApplicationNumber = x.EntityName,
                Description = x.Action,
                Date = x.Timestamp
            })
            .ToListAsync();

        var dashboard = new JamaatPresidentDashboardViewModel
        {
            PresidentName = User.Identity?.Name ?? "Jama'at President",

            // These can later come from the President's actual Jama'at profile.
            JamaatName = "Jama'at",
            CircuitName = "Circuit",

            PendingNikahReviews = pendingApplications.Count,

            ReviewedToday = reviewedToday,

            TotalNikahApplications = totalApplications,

            PendingApplications = pendingApplications
                .Select(x => new NikahApplicationViewModel
                {
                    Id = x.Id,

                    ReferenceNumber =
                        x.MarriageApplicationForm?.ReferenceNumber
                        ?? "N/A",

                    GroomName =
                        x.MarriageApplicationForm?.BridegroomName
                        ?? "Not provided",

                    BrideName =
                        x.MarriageApplicationForm?.BrideName
                        ?? "Not provided",

                    JamaatName =
                        x.MarriageApplicationForm?.Venue
                        ?? "Not provided",

                    SubmittedDate = x.CreatedAt,

                    Status = x.Status.ToString()
                })
                .ToList(),

            RecentActivities = recentActivities
        };

        return View(dashboard);
    }

    // ============================================================
    // REVIEW APPLICATION
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> Review(Guid id)
    {
        var application = await _context.Applications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        var form = application.MarriageApplicationForm;

        if (form == null)
        {
            return NotFound("Marriage application form was not found.");
        }

        var model = new JamaatPresidentReviewViewModel
        {
            Id = application.Id,
            ReferenceNumber = form.ReferenceNumber,
            Status = application.Status.ToString(),
            SubmittedDate = application.CreatedAt,
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

        return View(model);
    }

    // ============================================================
    // APPROVE
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id)
    {
        var application = await _context.Applications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Status !=
            ApplicationStatus.ApplicationPending)
        {
            TempData["Error"] =
                "This application is no longer awaiting Jama'at President review.";

            return RedirectToAction(nameof(Dashboard));
        }

        application.Status =
            ApplicationStatus.ApplicationApproved;

        application.ModifiedAt = DateTime.UtcNow;
        application.ModifiedBy = GetCurrentUserId();

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),

            UserId = GetCurrentUserId() ?? Guid.Empty,

            Action = "Approved Nikah Application",

            EntityName = "MarriageApplication",

            RecordId = application.Id,

            Timestamp = DateTime.UtcNow,

            ChangeDetails =
                $"Jama'at President approved application " +
                $"{application.MarriageApplicationForm?.ReferenceNumber ?? application.Id.ToString()} " +
                $"and forwarded it for National Rishtanata Secretary review."
        };

        _context.AuditLogs.Add(auditLog);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Nikah application approved and forwarded to the National Rishtanata Secretary.";

        return RedirectToAction(nameof(Dashboard));
    }

    // ============================================================
    // REJECT
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(Guid id)
    {
        var application = await _context.Applications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Status !=
            ApplicationStatus.ApplicationPending)
        {
            TempData["Error"] =
                "This application is no longer awaiting Jama'at President review.";

            return RedirectToAction(nameof(Dashboard));
        }

        application.Status =
            ApplicationStatus.ApplicationRejected;

        application.ModifiedAt = DateTime.UtcNow;
        application.ModifiedBy = GetCurrentUserId();

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),

            UserId = GetCurrentUserId() ?? Guid.Empty,

            Action = "Rejected Nikah Application",

            EntityName = "MarriageApplication",

            RecordId = application.Id,

            Timestamp = DateTime.UtcNow,

            ChangeDetails =
                $"Jama'at President rejected application " +
                $"{application.MarriageApplicationForm?.ReferenceNumber ?? application.Id.ToString()}."
        };

        _context.AuditLogs.Add(auditLog);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Nikah application has been rejected.";

        return RedirectToAction(nameof(Dashboard));
    }

    // ============================================================
    // REQUEST MORE INFORMATION
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestMoreInformation(Guid id)
    {
        var application = await _context.Applications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Status !=
            ApplicationStatus.ApplicationPending)
        {
            TempData["Error"] =
                "This application is no longer awaiting Jama'at President review.";

            return RedirectToAction(nameof(Dashboard));
        }

        application.Status =
            ApplicationStatus.ApplicationPending;

        application.ModifiedAt = DateTime.UtcNow;
        application.ModifiedBy = GetCurrentUserId();

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),

            UserId = GetCurrentUserId() ?? Guid.Empty,

            Action = "Requested More Information",

            EntityName = "MarriageApplication",

            RecordId = application.Id,

            Timestamp = DateTime.UtcNow,

            ChangeDetails =
                $"Jama'at President requested more information for application " +
                $"{application.MarriageApplicationForm?.ReferenceNumber ?? application.Id.ToString()}."
        };

        _context.AuditLogs.Add(auditLog);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "More information has been requested for this Nikah application.";

        return RedirectToAction(nameof(Dashboard));
    }

    // ============================================================
    // CURRENT USER
    // ============================================================

    private Guid? GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userId, out var id))
        {
            return id;
        }

        return null;
    }
}