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
using Domain.Enums;
using Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

namespace Presentation.Controllers;

[Authorize(Policy = "RequireRishtanataSecretary")]
public class RishtanataSecretaryController : Controller
{
    private readonly IRishtanataSecretaryService _service;
    private readonly ISharedSectionService _sharedSectionService;
    private readonly IMarriageApplicationFormService _formService;

    public RishtanataSecretaryController(
        IRishtanataSecretaryService service,
        ISharedSectionService sharedSectionService,
        IMarriageApplicationFormService formService)
    {
        _service = service;
        _sharedSectionService = sharedSectionService;
        _formService = formService;
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

    [HttpGet("SectionLinks/{id:guid}")]
    public async Task<IActionResult> SectionLinks(Guid id, CancellationToken ct)
    {
        var form = await _formService.GetByIdAsync(id, ct);
        if (form is null)
            return NotFound("Application not found.");

        var statuses = await _sharedSectionService.GetSignatureLinksStatusAsync(form.Id, ct);

        var model = new SecretarySectionLinksViewModel
        {
            ApplicationId = form.Id,
            ReferenceNumber = form.ReferenceNumber,
            BrideName = form.BrideName,
            BridegroomName = form.BridegroomName,
            FormStage = form.FormStage,
            Items = statuses.ToList()
        };

        return View(model);
    }

    [HttpPost("RegenerateLink")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegenerateLink(Guid applicationId, SectionType section, CancellationToken ct)
    {
        var form = await _formService.GetByIdAsync(applicationId, ct);
        if (form is null)
            return NotFound("Application not found.");

        var membershipNo = User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? string.Empty;

        string? rawToken = null;
        try
        {
            rawToken = await _sharedSectionService.RegenerateSectionTokenAsync(
                form.Id, section, membershipNo, ct);
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        var statuses = await _sharedSectionService.GetSignatureLinksStatusAsync(form.Id, ct);
        var sectionLabel = section switch
        {
            SectionType.Guardian => "Guardian / Waliy",
            SectionType.WitnessOne => "Witness 1",
            SectionType.WitnessTwo => "Witness 2",
            _ => section.ToString()
        };

        var model = new SecretarySectionLinksViewModel
        {
            ApplicationId = form.Id,
            ReferenceNumber = form.ReferenceNumber,
            BrideName = form.BrideName,
            BridegroomName = form.BridegroomName,
            FormStage = form.FormStage,
            Items = statuses.ToList(),
            RegeneratedSection = sectionLabel,
            RegeneratedUrl = rawToken is not null
                ? Url.Action("Fill", "SharedSection", new { token = rawToken }, Request.Scheme)
                : null
        };

        return View("SectionLinks", model);
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