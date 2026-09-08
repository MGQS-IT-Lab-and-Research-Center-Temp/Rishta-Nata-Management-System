using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Presentation.Mapping.RishtanataSecretary;
using Presentation.Mapping.JamaatMember;
using Application.Interfaces;
using Domain.Constants;

namespace Presentation.Controllers;

[Authorize(Policy = "RequireRishtanataSecretary")]
public class RishtanataSecretaryController : Controller
{
    private readonly IRishtanataSecretaryService _service;

    public RishtanataSecretaryController(
        IRishtanataSecretaryService service)
    {
        _service = service;
    }

    // Dashboard page
    public IActionResult Dashboard()
    {
        var dto = _service.GetDashboard(
            User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name));

        var model = RishtanataSecretaryDashboardMapping.ToViewModel(dto);

        return View(model);
    }

    // Pending approvals page
    public IActionResult PendingApprovals()
    {
        var pendingApprovals = _service.GetPendingApprovals();

        var viewModels = pendingApprovals
            .Select(PendingApprovalMapping.ToViewModel)
            .ToList();

        return View(viewModels);
    }

    // MarriedCouples Page
    public IActionResult MarriedCouples()
    {
        var marriedCouples = _service.GetMarriedCouples();

        var viewModels = marriedCouples
            .Select(MarriedCoupleMapping.ToViewModel)
            .ToList();

        return View(viewModels);
    }

    // View all Jama'at members
    public IActionResult JamaatMembers()
    {
        var members = _service.GetMembers();

        var viewModels = members
            .Select(JamaatMemberMapping.ToViewModel)
            .ToList();

        return View(viewModels);
    }

    // Review a specific application
    public IActionResult Review(Guid id)
    {
        var application = _service.GetById(id);

        if (application is null)
        {
            return NotFound("Application not found.");
        }

        var viewModel = RishtanataSecretaryReviewMapping.ToViewModel(application);

        return View(viewModel);
    }

    // Full member profile page
    public IActionResult MemberProfile(Guid id)
    {
        var dto = _service.GetMemberProfile(id);

        if (dto is null)
        {
            return NotFound("Member not found.");
        }

        var model = MemberProfileMapping.ToViewModel(dto);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id)
    {
        // Await the status change so the redirect can't beat the write (the
        // service now persists asynchronously instead of fire-and-forget).
        await _service.Approve(id);

        return RedirectToAction(nameof(PendingApprovals));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(Guid id)
    {
        // Same as Approve — wait for the write before redirecting.
        await _service.Reject(id);

        return RedirectToAction(nameof(PendingApprovals));
    }
}