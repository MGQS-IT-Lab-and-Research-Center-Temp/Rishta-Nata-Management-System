using System.Security.Claims;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.RishtanataSecretary;

namespace Presentation.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IMemberDashboardService _memberDashboardService;

    public ProfileController(IMemberDashboardService memberDashboardService)
    {
        _memberDashboardService = memberDashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var membershipNo = User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name);

        var dto = await _memberDashboardService.GetProfileAsync(
            membershipNo ?? string.Empty);

        if (dto is null)
        {
            return NotFound();
        }

        return View(MemberProfileMapping.ToViewModel(dto));
    }
}
