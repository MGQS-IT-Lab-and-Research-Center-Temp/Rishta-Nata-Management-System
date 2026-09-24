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
    private readonly IMemberLookupService _memberLookup;

    public SharedSectionController(
        ISharedSectionService sharedSectionService,
        IMemberLookupService memberLookup)
    {
        _sharedSectionService = sharedSectionService;
        _memberLookup = memberLookup;
    }

    [HttpGet("Fill/{token}")]
    public async Task<IActionResult> Fill(string token, CancellationToken ct)
    {
        var status = await _sharedSectionService.ValidateTokenAsync(token, ct);
        if (!status.IsValid)
        {
            if (status.IsSubmitted)
                return View("Submitted");

            return View("Invalid");
        }

        var model = new SectionFillViewModel
        {
            Token = token,
            SectionType = status.SectionType,
            ReferenceNumber = status.ReferenceNumber,
            BrideName = status.BrideName,
            BridegroomName = status.BridegroomName,
            SectionLabel = SectionTitle(status.SectionType),
            AppointsRepresentative = status.SectionType == SectionType.Guardian && status.AppointsRepresentative
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
            AppointsRepresentative = model.SectionType == SectionType.Guardian && model.AppointsRepresentative,
            IsMember = model.IsMember,
            MemberMembershipNo = model.MemberMembershipNo,
            SignatureDate = DateTime.UtcNow
        };

        var result = await _sharedSectionService.SubmitSectionAsync(model.Token, data, ct);

        if (!result.Success)
            return View("Invalid");

        TempData["StageAdvanced"] = result.StageAdvanced.ToString();
        return View("ThankYou");
    }

    [HttpGet("MemberLookup")]
    public async Task<IActionResult> MemberLookup(string token, string chandaNo, CancellationToken ct)
    {
        var status = await _sharedSectionService.ValidateTokenAsync(token, ct);
        if (!status.IsValid)
            return NotFound();

        if (string.IsNullOrWhiteSpace(chandaNo))
            return NotFound();

        var member = await _memberLookup.LookupAsync(chandaNo, ct);
        if (member is null)
            return NotFound();

        return Ok(new
        {
            chandaNo = member.ChandaNo,
            fullName = member.FullName,
            address = member.Address,
            phoneNo = member.PhoneNo
        });
    }

    [HttpGet("ThankYou")]
    public IActionResult ThankYou() => View();

    private static string SectionTitle(SectionType section) => section switch
    {
        SectionType.Guardian => "Guardian / Waliy",
        SectionType.WitnessOne => "Witness 1 — Guardian's Agreement",
        SectionType.WitnessTwo => "Witness 2 — Guardian's Agreement",
        SectionType.GroomWakeel => "Groom's Wakeel",
        SectionType.Representative => "Guardian's Representative (Wakeel)",
        SectionType.WakeelAppointmentWitnessOne => "Witness 1 — Wakeel Appointment",
        SectionType.WakeelAppointmentWitnessTwo => "Witness 2 — Wakeel Appointment",
        SectionType.GroomDeclarationWitnessOne => "Witness 1 — Groom's Declaration",
        SectionType.GroomDeclarationWitnessTwo => "Witness 2 — Groom's Declaration",
        SectionType.NikahCeremonyWitnessOne => "Witness 1 — Nikah Ceremony",
        SectionType.NikahCeremonyWitnessTwo => "Witness 2 — Nikah Ceremony",
        _ => section.ToString()
    };
}