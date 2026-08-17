using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

// TODO: Restore [Authorize(Roles = "JamaatPresident")] once authentication is built.
// Temporarily removed to allow testing this dashboard without a working login flow.
public class JamaatPresidentController : Controller
{
    private readonly IJamaatPresidentService _service;

    public JamaatPresidentController(IJamaatPresidentService service)
    {
        _service = service;
    }

    // ============================================================
    // DASHBOARD
    // ============================================================

    public async Task<IActionResult> Dashboard()
    {
        var dto = await _service.GetDashboardAsync(User.Identity?.Name);
        return View(dto);
    }

    // ============================================================
    // REVIEW APPLICATION
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> Review(Guid id)
    {
        var dto = await _service.GetReviewAsync(id);

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