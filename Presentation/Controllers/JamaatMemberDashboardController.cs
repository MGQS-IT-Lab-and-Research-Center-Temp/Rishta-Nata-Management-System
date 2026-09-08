using System.Security.Claims;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Authorize]
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
                .ToList(),
            ActiveApplication = dto.ActiveApplication is null
                ? null
                : new MemberApplicationViewModel
                {
                    Id = dto.ActiveApplication.Id,
                    ReferenceNumber = dto.ActiveApplication.ReferenceNumber,
                    SpouseName = dto.ActiveApplication.SpouseName,
                    Role = dto.ActiveApplication.Role,
                    Status = dto.ActiveApplication.Status,
                    SubmittedDate = dto.ActiveApplication.SubmittedDate,
                    IsAwaitingYourSection = dto.ActiveApplication.IsAwaitingYourSection,
                    FormStage = dto.ActiveApplication.FormStage
                }
        };

        return View(model);
    }
}
