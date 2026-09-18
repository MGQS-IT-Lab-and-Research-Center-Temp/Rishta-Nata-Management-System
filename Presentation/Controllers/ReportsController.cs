using System.Linq;
using System.Security.Claims;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.JamaatMember;
using Presentation.Mapping.RishtanataSecretary;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Authorize(Policy = "RequireRishtanataSecretary")]
public class ReportsController : Controller
{
    private readonly IRishtanataSecretaryService _service;

    public ReportsController(IRishtanataSecretaryService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var dashboard = _service.GetDashboard(
            User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name));

        return View(new ReportsViewModel
        {
            PendingApprovals = dashboard.PendingApprovals,
            ApprovedApplications = dashboard.ApprovedApplications,
            RejectedApplications = dashboard.RejectedApplications,
            MarriedCouples = dashboard.MarriedCouples,
            TotalMembers = dashboard.TotalMembers,
            MarriedCouplesList = _service.GetMarriedCouples()
                .Select(MarriedCoupleMapping.ToViewModel)
                .ToList(),
            JamaatMembersList = _service.GetMembers()
                .Select(JamaatMemberMapping.ToViewModel)
                .ToList()
        });
    }
}
