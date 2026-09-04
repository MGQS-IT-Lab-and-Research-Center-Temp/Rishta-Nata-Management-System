using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Presentation.Mapping.RishtanataSecretary;
using Presentation.Mapping.JamaatMember;
using Application.Interfaces;

namespace Presentation.Controllers;

[Authorize(Policy = "RequireRishtanataSecretary")]
public class RishtanataSecretaryController : Controller
{
    private readonly IRishtanataSecretaryService _service;
    private readonly IRoleAssignmentService _roleService;

    public RishtanataSecretaryController(
        IRishtanataSecretaryService service,
        IRoleAssignmentService roleService)
    {
        _service = service;
        _roleService = roleService;
    }

    // Dashboard page
    public IActionResult Dashboard()
    {
        var dto = _service.GetDashboard();

        var model = RishtanataSecretaryDashboardMapping.ToViewModel(dto);

        return View(model);
    }

    // Pending approvals page
    public async Task<IActionResult> PendingApprovals()
    {
        var pendingApprovals = _service.GetPendingApprovals();

        var viewModels = pendingApprovals
            .Select(PendingApprovalMapping.ToViewModel)
            .ToList();

        return View(viewModels);
    }

    // MarriedCouples Page
    public async Task<IActionResult> MarriedCouples()
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
    public async Task<IActionResult> Review(Guid id)
    {
        var application = _service.GetById(id);

        var viewModel = RishtanataSecretaryReviewMapping.ToViewModel(application);

        return View(viewModel);
    }

    // Full member profile page
    public async Task<IActionResult> MemberProfile(Guid id)
    {
        var dto = _service.GetMemberProfile(id);

        var model = MemberProfileMapping.ToViewModel(dto);

        return View(model);
    }

    // Edit Role of a specific Jamaat Member
    public async Task<IActionResult> EditRoles(Guid id)
    {
        var dto = await _roleService.GetRoleManagementAsync(id);

        var viewModel = RoleManagementMapper.ToViewModel(dto);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(Guid id)
    {
        // Await the status change so the redirect can't beat the write (the
        // service now persists asynchronously instead of fire-and-forget).
        await _service.Approve(id);

        return RedirectToAction(nameof(PendingApprovals));
    }

    [HttpPost]
    public async Task<IActionResult> Reject(Guid id)
    {
        // Same as Approve — wait for the write before redirecting.
        await _service.Reject(id);

        return RedirectToAction(nameof(PendingApprovals));
    }
}