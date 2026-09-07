using Application.Interfaces;
using Domain.Enums;
using Infrastructure.DTOs.SharedSection;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

/// <summary>
/// Fully anonymous fill entry points for the guardian and witness sections at
/// the AwaitingWitnesses stage. Validity is entirely token-scoped
/// (docs/stage-authorization-policy.md §8).
/// </summary>
[Route("SharedSection")]
public class SharedSectionController : Controller
{
    private readonly ISharedSectionService _sharedSectionService;

    public SharedSectionController(ISharedSectionService sharedSectionService)
    {
        _sharedSectionService = sharedSectionService;
    }

    [HttpGet("Fill/{token}")]
    public async Task<IActionResult> Fill(string token, CancellationToken ct)
    {
        var status = await _sharedSectionService.ValidateTokenAsync(token, ct);
        if (!status.IsValid)
            return View("Invalid");

        var model = new SectionFillViewModel
        {
            Token = token,
            SectionType = status.SectionType,
            ReferenceNumber = status.ReferenceNumber,
            BrideName = status.BrideName,
            BridegroomName = status.BridegroomName,
            SectionLabel = SectionTitle(status.SectionType)
        };

        return View(model);
    }

    [HttpPost("Fill")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Fill(SectionFillViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var data = new SectionFillData
        {
            Name = model.Name ?? string.Empty,
            Address = model.Address ?? string.Empty,
            Tel = model.Tel ?? string.Empty,
            RelationToBride = model.RelationToBride ?? string.Empty,
            IsMember = model.IsMember,
            MemberMembershipNo = model.MemberMembershipNo,
            SignatureDate = model.SignatureDate ?? DateTime.UtcNow
        };

        var result = await _sharedSectionService.SubmitSectionAsync(model.Token, data, ct);

        if (!result.Success)
            return View("Invalid");

        TempData["StageAdvanced"] = result.StageAdvanced.ToString();
        return View("ThankYou");
    }

    [HttpGet("ThankYou")]
    public IActionResult ThankYou() => View();

    private static string SectionTitle(SectionType section) => section switch
    {
        SectionType.Guardian => "Guardian / Waliy",
        SectionType.WitnessOne => "Witness 1",
        SectionType.WitnessTwo => "Witness 2",
        _ => section.ToString()
    };
}