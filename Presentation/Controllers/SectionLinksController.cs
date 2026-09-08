using System.Security.Claims;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Infrastructure.DTOs.SharedSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Authorize]
[Route("SectionLinks")]
public class SectionLinksController : Controller
{
    private readonly IMarriageApplicationFormService _formService;
    private readonly ISharedSectionService _sharedSectionService;

    public SectionLinksController(
        IMarriageApplicationFormService formService,
        ISharedSectionService sharedSectionService)
    {
        _formService = formService;
        _sharedSectionService = sharedSectionService;
    }

    [HttpGet("Index/{applicationId:guid}")]
    public async Task<IActionResult> Index(Guid applicationId, CancellationToken ct)
    {
        var form = await _formService.GetByIdAsync(applicationId, ct);
        if (form is null)
            return RedirectToAction("Index", "Applications");

        var membershipNo = MembershipNo();
        if (!IsParty(form, membershipNo))
            return RedirectToAction("Index", "Applications");

        var statuses = await _sharedSectionService.GetSignatureLinksStatusAsync(form.Id, ct);

        var model = new SectionLinksViewModel
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

    [HttpPost("Generate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generate(Guid applicationId, SectionType section, CancellationToken ct)
    {
        var form = await _formService.GetByIdAsync(applicationId, ct);
        if (form is null || !IsParty(form, MembershipNo()))
            return RedirectToAction("Index", "Applications");

        var membershipNo = MembershipNo();

        try
        {
            var raw = await _sharedSectionService.GenerateSectionTokenAsync(
                form.Id, section, membershipNo, ct);
            TempData["RawLink"] = BuildFillUrl(raw);
            TempData["SectionLabel"] = SectionTitle(section);
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { applicationId });
    }

    [HttpPost("Regenerate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Regenerate(Guid applicationId, SectionType section, CancellationToken ct)
    {
        var form = await _formService.GetByIdAsync(applicationId, ct);
        if (form is null || !IsParty(form, MembershipNo()))
            return RedirectToAction("Index", "Applications");

        var membershipNo = MembershipNo();

        try
        {
            var raw = await _sharedSectionService.RegenerateSectionTokenAsync(
                form.Id, section, membershipNo, ct);
            TempData["RawLink"] = BuildFillUrl(raw);
            TempData["SectionLabel"] = SectionTitle(section);
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { applicationId });
    }

    private string BuildFillUrl(string raw) =>
        Url.Action("Fill", "SharedSection", new { token = raw }, Request.Scheme)!;

    private static string SectionTitle(SectionType section) => section switch
    {
        SectionType.Guardian => "Guardian / Waliy",
        SectionType.WitnessOne => "Witness 1",
        SectionType.WitnessTwo => "Witness 2",
        _ => section.ToString()
    };

    private static bool IsParty(MarriageApplicationForm form, string membershipNo) =>
        !string.IsNullOrWhiteSpace(membershipNo) &&
        (string.Equals(form.BrideMembershipNo, membershipNo, StringComparison.OrdinalIgnoreCase) ||
         string.Equals(form.BridegroomMembershipNo, membershipNo, StringComparison.OrdinalIgnoreCase));

    private string MembershipNo() =>
        User.FindFirstValue(ClaimNames.MembershipNo)
        ?? User.FindFirstValue(ClaimTypes.Name)
        ?? string.Empty;
}