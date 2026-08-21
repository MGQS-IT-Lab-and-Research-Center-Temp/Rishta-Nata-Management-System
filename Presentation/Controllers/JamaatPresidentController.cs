//// do page for review - azeez
//// do page for review for individual nikkah form - azeez
//// do page for viewing aqeeqah certificates - yusroh - done
//// do page for viewing all certificates under the jama'at president's jama'at (for now view all certificates) - faridah
//// fix all errors under your dto - faridah -done
//// fix all errors under service and interface - yusroh
//// ensure that dto namespace is infrastructure not application - done

//using Domain.Entities;
//using Domain.Enums;
//using Infrastructure.Persistence;
//using Infrastructure.DTOs.Certificates;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Presentation.ViewModels;
//using System.Security.Claims;
//using Application.Interfaces;

//namespace Presentation.Controllers;

//// TODO: Restore [Authorize(Roles = "JamaatPresident")] once authentication is built.
//// Temporarily removed to allow testing this dashboard without a working login flow.
//public class JamaatPresidentController : Controller
//{
//    private readonly RishtanataDbContext _context;
//    private readonly IAqeeqahCertificateService _aqeeqahService;

//    public JamaatPresidentController(RishtanataDbContext context, IAqeeqahCertificateService aqeeqahService)
//    {
//        _context = context;
//        _aqeeqahService = aqeeqahService;
//    }

//    // ============================================================
//    // DASHBOARD
//    // ============================================================

//    public async Task<IActionResult> Dashboard()
//    {
//        var pendingStatus = ApplicationStatus.ApplicationPending;

//        var pendingApplications = await _context.FormApplications
//            .Where(x => x.Status == pendingStatus)
//            .Include(x => x.MarriageApplicationForm)
//            .OrderByDescending(x => x.CreatedAt)
//            .ToListAsync();

//        var totalApplications =
//            await _context.FormApplications.CountAsync();

//        var today = DateTime.UtcNow.Date;
//        var tomorrow = today.AddDays(1);

//        var reviewedToday = await _context.AuditLogs
//            .CountAsync(x =>
//                x.Timestamp >= today &&
//                x.Timestamp < tomorrow &&
//                (
//                    x.Action == "Approved Nikah Application" ||
//                    x.Action == "Rejected Nikah Application" ||
//                    x.Action == "Requested More Information"
//                ));

//        var recentActivities = await _context.AuditLogs
//            .OrderByDescending(x => x.Timestamp)
//            .Take(10)
//            .Select(x => new RecentActivityViewModel
//            {
//                ApplicationNumber = x.EntityName,
//                Description = x.Action,
//                Date = x.Timestamp
//            })
//            .ToListAsync();

//        var dashboard = new JamaatPresidentDashboardViewModel
//        {
//            PresidentName = User.Identity?.Name ?? "Jama'at President",

//            // These can later come from the President's actual Jama'at profile.
//            JamaatName = "Jama'at",
//            CircuitName = "Circuit",

//            PendingNikahReviews = pendingApplications.Count,

//            ReviewedToday = reviewedToday,

//            TotalNikahApplications = totalApplications,

//            PendingApplications = pendingApplications
//                .Select(x => new NikahApplicationViewModel
//                {
//                    Id = x.Id,

//                    ReferenceNumber =
//                        x.MarriageApplicationForm?.ReferenceNumber
//                        ?? "N/A",

//                    GroomName =
//                        x.MarriageApplicationForm?.BridegroomName
//                        ?? "Not provided",

//                    BrideName =
//                        x.MarriageApplicationForm?.BrideName
//                        ?? "Not provided",

//                    JamaatName =
//                        x.MarriageApplicationForm?.Venue
//                        ?? "Not provided",

//                    SubmittedDate = x.CreatedAt,

//                    Status = x.Status.ToString()
//                })
//                .ToList(),

//            RecentActivities = recentActivities
//        };

//        return View(dashboard);
//    }

//    // ============================================================
//    // REVIEW APPLICATION
//    // ============================================================

//    [HttpGet]
//    public async Task<IActionResult> Review(Guid id)
//    {
//        var application = await _context.FormApplications
//            .Include(x => x.MarriageApplicationForm)
//            .FirstOrDefaultAsync(x => x.Id == id);

//        if (application == null)
//        {
//            return NotFound();
//        }

//        var form = application.MarriageApplicationForm;

//        if (form == null)
//        {
//            return NotFound("Marriage application form was not found.");
//        }

//        {var model = new JamaatPresidentReviewViewModel
//{
//         Id = application.Id,
//         ReferenceNumber = form.ReferenceNumber,
//         Status = application.Status.ToString(),
//         SubmittedDate = application.CreatedAt,
//         ProposedNikahDate = form.ProposedNikahDate,
//         Venue = form.Venue,

//        BrideMembershipNo = form.Bride?.MembershipNo ?? string.Empty,
//        BrideName = form.Bride?.Name ?? string.Empty,
//        BrideDateOfBirth = form.Bride?.DateOfBirth ?? DateTime.MinValue,
//        BrideResidentOf = form.Bride?.ResidentOf ?? string.Empty,
//        BrideGenotype = form.Bride?.Genotype ?? string.Empty,
//        BrideBloodGroup = form.Bride?.BloodGroup ?? string.Empty,
//        BrideMaritalStatus = form.Bride?.MaritalStatus ?? string.Empty,
//        BrideProposedDowerAmount = form.Bride?.ProposedDowerAmount ?? 0,
//        BrideDowerAmountReceivedInCash = form.Bride?.DowerAmountReceivedInCash ?? 0,
//        BrideSignatureTel = form.Bride?.SignatureTel ?? string.Empty,
//};


//            Bridegroom = new Bridegroom
//            {
//                MembershipNo = form.BridegroomMembershipNo,
//                Name = form.BridegroomName,
//                DateOfBirth = form.BridegroomDateOfBirth,
//                ResidentOf = form.BridegroomResidentOf,
//                Genotype = form.BridegroomGenotype,
//                BloodGroup = form.BridegroomBloodGroup,
//                DowerAmountPaidInCash = form.BridegroomDowerAmountPaidInCash,
//                DowerAmountToBePaid = form.BridegroomDowerAmountToBePaid,
//                IsFirstNikah = form.IsFirstNikah,
//                IsSecondThirdOrFourthNikah = form.IsSecondThirdOrFourthNikah,
//                FormerWifeIsDead = form.FormerWifeIsDead,
//                HasDivorcedFormerWife = form.HasDivorcedFormerWife,
//                FormerWifeIsPresent = form.FormerWifeIsPresent,
//                FormerWifeObtainedKhula = form.FormerWifeObtainedKhula,
//                SignatureTel = form.BridegroomSignatureTel,
//                FatherName = form.BridegroomFatherName
//            },

//Bride = new Bride
//{
//    FatherName = form.BrideFatherName
//},

//Guardian = new Guardian
//{
//    Name = form.GuardianName,
//    RelationToBride = form.GuardianRelationToBride,
//    Address = form.GuardianAddress,
//    Tel = form.GuardianTel,
//    SignatureDate = form.GuardianSignatureDate
//},

//Representative = new Representative
//{
//    Name = form.RepresentativeName,
//    Address = form.RepresentativeAddress,
//    ActingFor = form.RepresentativeActingFor,
//    SignatureDate = form.RepresentativeSignatureDate
//},

//WitnessOne = new Witness
//{
//    Name = form.WitnessOneName,
//    Address = form.WitnessOneAddress,
//    Tel = form.WitnessOneTel,
//    SignatureDate = form.WitnessOneSignatureDate
//},

//WitnessTwo = new Witness
//{
//    Name = form.WitnessTwoName,
//    Address = form.WitnessTwoAddress,
//    Tel = form.WitnessTwoTel,
//    SignatureDate = form.WitnessTwoSignatureDate
//},

//OfficiatingImam = new OfficiatingImam
//{
//    Name = form.OfficiatingImamName,
//    AddressJamaat = form.OfficiatingImamAddressJamaat,
//    SignatureDate = form.OfficiatingImamSignatureDate
//},

//JamaatPresident = new JamaatPresident
//{
//    Name = form.JamaatPresidentName,
//    SignatureDate = form.JamaatPresidentSignatureDate
//},

//NationalRishtanataSecretary = new NationalRishtanataSecretary
//{
//    Name = form.NationalRishtanataSecretaryName,
//    SignatureDate = form.NationalRishtanataSecretarySignatureDate
//},

//ApprovedDateOfNikah = form.ApprovedDateOfNikah,
//NationalAmirOrMissionarySignatureDate =
//    form.NationalAmirOrMissionarySignatureDate
//    }

//    // ============================================================
//    // APPROVE
//    // ============================================================

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Approve(Guid id)
//    {
//        var application = await _context.FormApplications
//            .Include(x => x.MarriageApplicationForm)
//            .FirstOrDefaultAsync(x => x.Id == id);

//        if (application == null)
//        {
//            return NotFound();
//        }

//        if (application.Status !=
//            ApplicationStatus.ApplicationPending)
//        {
//            TempData["Error"] =
//                "This application is no longer awaiting Jama'at President review.";

//            return RedirectToAction(nameof(Dashboard));
//        }

//        application.Status =
//            ApplicationStatus.ApplicationApproved;

//        application.ModifiedAt = DateTime.UtcNow;
//        application.ModifiedBy = GetCurrentUserId();

//        var auditLog = new AuditLog
//        {
//            Id = Guid.NewGuid(),

//            UserId = GetCurrentUserId() ?? Guid.Empty,

//            Action = "Approved Nikah Application",

//            EntityName = "MarriageApplication",

//            RecordId = application.Id,

//            Timestamp = DateTime.UtcNow,

//            ChangeDetails =
//                $"Jama'at President approved application " +
//                $"{application.MarriageApplicationForm?.ReferenceNumber ?? application.Id.ToString()} " +
//                $"and forwarded it for National Rishtanata Secretary review."
//        };

//        _context.AuditLogs.Add(auditLog);

//        await _context.SaveChangesAsync();

//        TempData["Success"] =
//            "Nikah application approved and forwarded to the National Rishtanata Secretary.";

//        return RedirectToAction(nameof(Dashboard));
//    }

//    // ============================================================
//    // REJECT
//    // ============================================================

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Reject(Guid id)
//    {
//        var application = await _context.FormApplications
//            .Include(x => x.MarriageApplicationForm)
//            .FirstOrDefaultAsync(x => x.Id == id);

//        if (application == null)
//        {
//            return NotFound();
//        }

//        if (application.Status !=
//            ApplicationStatus.ApplicationPending)
//        {
//            TempData["Error"] =
//                "This application is no longer awaiting Jama'at President review.";

//            return RedirectToAction(nameof(Dashboard));
//        }

//        application.Status =
//            ApplicationStatus.ApplicationRejected;

//        application.ModifiedAt = DateTime.UtcNow;
//        application.ModifiedBy = GetCurrentUserId();

//        var auditLog = new AuditLog
//        {
//            Id = Guid.NewGuid(),

//            UserId = GetCurrentUserId() ?? Guid.Empty,

//            Action = "Rejected Nikah Application",

//            EntityName = "MarriageApplication",

//            RecordId = application.Id,

//            Timestamp = DateTime.UtcNow,

//            ChangeDetails =
//                $"Jama'at President rejected application " +
//                $"{application.MarriageApplicationForm?.ReferenceNumber ?? application.Id.ToString()}."
//        };

//        _context.AuditLogs.Add(auditLog);

//        await _context.SaveChangesAsync();

//        TempData["Success"] =
//            "Nikah application has been rejected.";

//        return RedirectToAction(nameof(Dashboard));
//    }

//    // ============================================================
//    // REQUEST MORE INFORMATION
//    // ============================================================

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> RequestMoreInformation(Guid id)
//    {
//        var application = await _context.FormApplications
//            .Include(x => x.MarriageApplicationForm)
//            .FirstOrDefaultAsync(x => x.Id == id);

//        if (application == null)
//        {
//            return NotFound();
//        }

//        if (application.Status !=
//            ApplicationStatus.ApplicationPending)
//        {
//            TempData["Error"] =
//                "This application is no longer awaiting Jama'at President review.";

//            return RedirectToAction(nameof(Dashboard));
//        }

//        application.Status =
//            ApplicationStatus.ApplicationPending;

//        application.ModifiedAt = DateTime.UtcNow;
//        application.ModifiedBy = GetCurrentUserId();

//        var auditLog = new AuditLog
//        {
//            Id = Guid.NewGuid(),

//            UserId = GetCurrentUserId() ?? Guid.Empty,

//            Action = "Requested More Information",

//            EntityName = "MarriageApplication",

//            RecordId = application.Id,

//            Timestamp = DateTime.UtcNow,

//            ChangeDetails =
//                $"Jama'at President requested more information for application " +
//                $"{application.MarriageApplicationForm?.ReferenceNumber ?? application.Id.ToString()}."
//        };

//        _context.AuditLogs.Add(auditLog);

//        await _context.SaveChangesAsync();

//        TempData["Success"] =
//            "More information has been requested for this Nikah application.";

//        return RedirectToAction(nameof(Dashboard));
//    }

//    // ============================================================
//    // MARRIAGE CERTIFICATES
//    // ============================================================

//    /// <summary>
//    /// Displays all marriage certificates.
//    /// 
//    /// For now, all certificates are displayed.
//    /// Later, this can be filtered by the Jama'at President's Jama'at.
//    /// </summary>
//    public async Task<IActionResult> Certificates()
//    {
//        var certificates = await _context.Certificates
//            .AsNoTracking()
//            .Select(c => new CertificateDto
//            {
//                Id = c.Id,
//                SerialNumber = c.SerialNumber,
//                BrideName = c.BrideName,
//                BridegroomName = c.BridegroomName,
//                NikahDate = c.NikahDate,
//                IssueDate = c.IssueDate,
//                CertificateFilePath = c.CertificateFilePath
//            })
//            .OrderByDescending(c => c.IssueDate)
//            .ToListAsync();

//        return View(certificates);
//    }

//    // ============================================================
//    // AQEEQAH CERTIFICATES
//    // ============================================================

//    /// <summary>
//    /// Displays all Aqeeqah certificates for the Jamaat President
//    /// </summary>
//    public async Task<IActionResult> AqeeqahCertificates()
//    {
//        var certificates = await _aqeeqahService.GetAllCertificatesAsync();
//        return View(certificates);
//    }

//    // ============================================================
//    // CURRENT USER
//    // ============================================================

//    private Guid? GetCurrentUserId()
//    {
//        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//        if (Guid.TryParse(userId, out var id))
//        {
//            return id;
//        }

//        return null;
//    }
//}



using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.Certificates;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.ViewModels;
using System.Security.Claims;
using Application.Interfaces;

namespace Presentation.Controllers;

public class JamaatPresidentController : Controller
{
    private readonly RishtanataDbContext _context;
    private readonly IAqeeqahCertificateService _aqeeqahService;

    public JamaatPresidentController(
        RishtanataDbContext context,
        IAqeeqahCertificateService aqeeqahService)
    {
        _context = context;
        _aqeeqahService = aqeeqahService;
    }

    // ============================================================
    // DASHBOARD
    // ============================================================

    public async Task<IActionResult> Dashboard()
    {
        var pendingStatus = ApplicationStatus.ApplicationPending;

        var pendingApplications = await _context.FormApplications
            .AsNoTracking()
            .Where(x => x.Status == pendingStatus)
            .Include(x => x.MarriageApplicationForm)
            .ToListAsync();

        var totalApplications =
            await _context.FormApplications.CountAsync();

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
            .AsNoTracking()
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

            // TODO: Replace these with the actual President's Jama'at details
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
                        x.MarriageApplicationForm?.Bride?.Name
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
        var application = await _context.FormApplications
            .AsNoTracking()
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

            // ====================================================
            // BRIDE
            // ====================================================

            BrideMembershipNo =
                form.Bride?.MembershipNo ?? string.Empty,

            BrideName =
                form.Bride?.Name ?? string.Empty,

            BrideDateOfBirth =
                form.Bride?.DateOfBirth ?? DateTime.MinValue,

            BrideResidentOf =
                form.Bride?.ResidentOf ?? string.Empty,

            BrideGenotype =
                form.Bride?.Genotype ?? string.Empty,

            BrideBloodGroup =
                form.Bride?.BloodGroup ?? string.Empty,

            BrideMaritalStatus =
                form.Bride?.MaritalStatus ?? string.Empty,

            BrideProposedDowerAmount =
                form.Bride?.ProposedDowerAmount ?? 0,

            BrideDowerAmountReceivedInCash =
                form.Bride?.DowerAmountReceivedInCash ?? 0,

            BrideSignatureTel =
                form.Bride?.SignatureTel ?? string.Empty,

            BrideFatherName =
                form.BrideFatherName ?? string.Empty,

            // ====================================================
            // BRIDEGROOM
            // ====================================================

            BridegroomMembershipNo =
                form.BridegroomMembershipNo ?? string.Empty,

            BridegroomName =
                form.BridegroomName ?? string.Empty,

            BridegroomDateOfBirth =
                form.BridegroomDateOfBirth,

            BridegroomResidentOf =
                form.BridegroomResidentOf ?? string.Empty,

            BridegroomGenotype =
                form.BridegroomGenotype ?? string.Empty,

            BridegroomBloodGroup =
                form.BridegroomBloodGroup ?? string.Empty,

            BridegroomDowerAmountPaidInCash =
                form.BridegroomDowerAmountPaidInCash,

            BridegroomDowerAmountToBePaid =
                form.BridegroomDowerAmountToBePaid,

            IsFirstNikah =
                form.IsFirstNikah,

            IsSecondThirdOrFourthNikah =
                form.IsSecondThirdOrFourthNikah,

            FormerWifeIsDead =
                form.FormerWifeIsDead,

            HasDivorcedFormerWife =
                form.HasDivorcedFormerWife,

            FormerWifeIsPresent =
                form.FormerWifeIsPresent,

            FormerWifeObtainedKhula =
                form.FormerWifeObtainedKhula,

            BridegroomSignatureTel =
                form.BridegroomSignatureTel ?? string.Empty,

            BridegroomFatherName =
                form.BridegroomFatherName ?? string.Empty,

            // ====================================================
            // GUARDIAN
            // ====================================================

            GuardianName =
                form.GuardianName ?? string.Empty,

            GuardianRelationToBride =
                form.GuardianRelationToBride ?? string.Empty,

            GuardianAddress =
                form.GuardianAddress ?? string.Empty,

            GuardianTel =
                form.GuardianTel ?? string.Empty,

            GuardianSignatureDate =
                form.GuardianSignatureDate,

            // ====================================================
            // REPRESENTATIVE
            // ====================================================

            RepresentativeName =
                form.RepresentativeName ?? string.Empty,

            RepresentativeAddress =
                form.RepresentativeAddress ?? string.Empty,

            RepresentativeActingFor =
                form.RepresentativeActingFor ?? string.Empty,

            RepresentativeSignatureDate =
                form.RepresentativeSignatureDate,

            // ====================================================
            // WITNESS ONE
            // ====================================================

            WitnessOneName =
                form.WitnessOneName ?? string.Empty,

            WitnessOneAddress =
                form.WitnessOneAddress ?? string.Empty,

            WitnessOneTel =
                form.WitnessOneTel ?? string.Empty,

            WitnessOneSignatureDate =
                form.WitnessOneSignatureDate,

            // ====================================================
            // WITNESS TWO
            // ====================================================

            WitnessTwoName =
                form.WitnessTwoName ?? string.Empty,

            WitnessTwoAddress =
                form.WitnessTwoAddress ?? string.Empty,

            WitnessTwoTel =
                form.WitnessTwoTel ?? string.Empty,

            WitnessTwoSignatureDate =
                form.WitnessTwoSignatureDate,

            // ====================================================
            // OFFICIATING IMAM
            // ====================================================

            OfficiatingImamName =
                form.OfficiatingImamName ?? string.Empty,

            OfficiatingImamAddressJamaat =
                form.OfficiatingImamAddressJamaat ?? string.Empty,

            OfficiatingImamSignatureDate =
                form.OfficiatingImamSignatureDate,

            // ====================================================
            // JAMA'AT PRESIDENT
            // ====================================================

            JamaatPresidentName =
                form.JamaatPresidentName ?? string.Empty,

            JamaatPresidentSignatureDate =
                form.JamaatPresidentSignatureDate,

            // ====================================================
            // NATIONAL RISHTANATA SECRETARY
            // ====================================================

            NationalRishtanataSecretaryName =
                form.NationalRishtanataSecretaryName ?? string.Empty,

            NationalRishtanataSecretarySignatureDate =
                form.NationalRishtanataSecretarySignatureDate,

            // ====================================================
            // FINAL APPROVAL
            // ====================================================

            ApprovedDateOfNikah =
                form.ApprovedDateOfNikah,

            NationalAmirOrMissionarySignatureDate =
                form.NationalAmirOrMissionarySignatureDate
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
        var application = await _context.FormApplications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Status != ApplicationStatus.ApplicationPending)
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
        var application = await _context.FormApplications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Status != ApplicationStatus.ApplicationPending)
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
        var application = await _context.FormApplications
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Status != ApplicationStatus.ApplicationPending)
        {
            TempData["Error"] =
                "This application is no longer awaiting Jama'at President review.";

            return RedirectToAction(nameof(Dashboard));
        }

        /*
         * The status is currently kept as Pending because there is
         * no separate "More Information Required" status in the
         * current ApplicationStatus enum.
         */
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
    // MARRIAGE CERTIFICATES
    // ============================================================

    public async Task<IActionResult> Certificates()
    {
        var certificates = await _context.Certificates
            .AsNoTracking()
            .Select(c => new CertificateDto
            {
                Id = c.Id,
                SerialNumber = c.SerialNumber,
                BrideName = c.BrideName,
                BridegroomName = c.BridegroomName,
                NikahDate = c.NikahDate,
                IssueDate = c.IssueDate,
                CertificateFilePath = c.CertificateFilePath
            })
            .OrderByDescending(c => c.IssueDate)
            .ToListAsync();

        return View(certificates);
    }

    // ============================================================
    // AQEEQAH CERTIFICATES
    // ============================================================

    public async Task<IActionResult> AqeeqahCertificates()
    {
        var certificates =
            await _aqeeqahService.GetAllCertificatesAsync();

        return View(certificates);
    }

    // ============================================================
    // CURRENT USER
    // ============================================================

    private Guid? GetCurrentUserId()
    {
        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userId, out var id))
        {
            return id;
        }

        return null;
    }
}