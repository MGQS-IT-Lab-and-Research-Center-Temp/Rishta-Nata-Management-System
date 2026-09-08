using System.Security.Claims;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Authorize]
public class ApplicationsController : Controller
{
    private readonly IMemberDashboardService _memberDashboardService;

    public ApplicationsController(IMemberDashboardService memberDashboardService)
    {
        _memberDashboardService = memberDashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var membershipNo = User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name);

        var applications = await _memberDashboardService.GetApplicationsAsync(
            membershipNo ?? string.Empty);

        var model = applications
            .Select(a => new MemberApplicationViewModel
            {
                Id = a.Id,
                ReferenceNumber = a.ReferenceNumber,
                SpouseName = a.SpouseName,
                Role = a.Role,
                Status = a.Status,
                SubmittedDate = a.SubmittedDate,
                IsAwaitingYourSection = a.IsAwaitingYourSection,
                FormStage = a.FormStage
            })
            .ToList();

        return View(model);
    }
}
