using System.Security.Claims;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class JamaatMemberDashboardController : Controller
{
    private readonly IMemberDashboardService _memberDashboardService;

    public JamaatMemberDashboardController(IMemberDashboardService memberDashboardService)
    {
        _memberDashboardService = memberDashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var membershipNo = User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name);

        var dto = await _memberDashboardService.GetDashboardAsync(membershipNo ?? string.Empty);

        var model = new MemberDashboardViewModel
        {
            MemberName = dto.MemberName,
            CurrentSpouse = dto.CurrentSpouse is null
                ? null
                : new SpouseInfo
                {
                    Name = dto.CurrentSpouse.Name,
                    MarriageDate = dto.CurrentSpouse.MarriageDate
                },
            MarriageHistory = dto.MarriageHistory
                .Select(entry => new MarriageHistoryEntry
                {
                    SpouseName = entry.SpouseName,
                    Status = entry.Status,
                    Date = entry.Date
                })
                .ToList()
        };

        return View(model);
    }
}
