
// do page for review for individual nikkah form - azeez
// do page for viewing aqeeqah certificates - yusroh - done
// do page for viewing all certificates under the jama'at president's jama'at (for now view all certificates) - faridah
// fix all errors under your dto - faridah -done
// fix all errors under service and interface - yusroh
// ensure that dto namespace is infrastructure not application - done
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
//use the respective service to do all db operation in this controller


using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.Certificates;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Constants.Roles;
using Presentation.ViewModels;
using System.Security.Claims;

namespace Presentation.Controllers;

[Authorize(Policy = "RequireJamaatSecretary")]

public class JamaatPresidentController : Controller
{
    private readonly IJamaatPresidentService _service;
    private readonly IAqeeqahCertificateService _aqeeqahService;

    public JamaatPresidentController(IJamaatPresidentService service, IAqeeqahCertificateService aqeeqahService)
    {
        _service = service;
        _aqeeqahService = aqeeqahService;
    }

    // ============================================================
    // DASHBOARD
    // ============================================================

    public async Task<IActionResult> Dashboard()
    {
        var pendingStatus = ApplicationStatus.ApplicationPending;

        var pendingApplications = await _context.FormApplications
            .Where(x => x.Status == pendingStatus)
            .Include(x => x.MarriageApplicationForm)
            .OrderByDescending(x => x.CreatedAt)
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

        return View(dto);
    }

    // ============================================================
    // REVIEW APPLICATION
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> Review(Guid id)
    {
        var dto = await _service.GetReviewByIdAsync(id);

        if (dto == null)
        {
            return NotFound("Marriage application or its form was not found.");
        }

        return View(dto);
    }

    // ============================================================
    // APPROVE
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id)
    {
        var success = await _service.ApproveAsync(id, GetCurrentUserId());

        TempData["Success"] = success
            ? "Nikah application approved and forwarded to the National Rishtanata Secretary."
            : null;

        TempData["Error"] = success
            ? null
            : "This application is no longer awaiting Jama'at President review.";

        return RedirectToAction(nameof(Dashboard));
    }

    // ============================================================
    // REJECT
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(Guid id)
    {
        var success = await _service.RejectAsync(id, GetCurrentUserId());

        TempData["Success"] = success
            ? "Nikah application has been rejected."
            : null;

        TempData["Error"] = success
            ? null
            : "This application is no longer awaiting Jama'at President review.";

        return RedirectToAction(nameof(Dashboard));
    }

    // ============================================================
    // REQUEST MORE INFORMATION
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestMoreInformation(Guid id)
    {
        var success = await _service.RequestMoreInformationAsync(id, GetCurrentUserId());

        TempData["Success"] = success
            ? "More information has been requested for this Nikah application."
            : null;

        TempData["Error"] = success
            ? null
            : "This application is no longer awaiting Jama'at President review.";

        return RedirectToAction(nameof(Dashboard));
    }

    // ============================================================
    // MARRIAGE CERTIFICATES
    // ============================================================

    /// <summary>
    /// Displays all marriage certificates.
    ///
    /// For now, all certificates are displayed.
    /// Later, this can be filtered by the Jama'at President's Jama'at.
    /// </summary>

        return View(certificates);
    }

    // ============================================================
    // AQEEQAH CERTIFICATES
    // ============================================================

    /// <summary>
    /// Displays all Aqeeqah certificates for the Jamaat President
    /// </summary>
    public async Task<IActionResult> AqeeqahCertificates()
    {
        var certificates = await _aqeeqahService.GetAllCertificatesAsync();
        return View(certificates);
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
