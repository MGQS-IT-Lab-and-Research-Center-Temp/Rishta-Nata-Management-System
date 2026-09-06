using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Authorize]
public class BridegroomController : Controller
{
    private readonly IMarriageApplicationFormService _formService;
    private readonly IMemberDashboardService _memberDashboardService;

    public BridegroomController(
        IMarriageApplicationFormService formService,
        IMemberDashboardService memberDashboardService)
    {
        _formService = formService;
        _memberDashboardService = memberDashboardService;
    }

    // GET: New Application (role-aware — either party can start)
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var model = new NewApplicationViewModel();

        var membershipNo = GetCurrentMembershipNo();

        if (!string.IsNullOrWhiteSpace(membershipNo))
        {
            var profile = await _memberDashboardService.GetProfileAsync(membershipNo, ct);

            if (profile is not null)
            {
                var isBride = IsFemale(profile.Sex);
                model.StartingParty = isBride ? "Bride" : "Groom";

                var starter = isBride ? model.Bride : model.Bridegroom;
                starter.MembershipNo = profile.ChandaNo ?? membershipNo;
                starter.Name = profile.FullName ?? string.Empty;
                starter.DateOfBirth = profile.DateOfBirth;
                starter.ResidentOf = profile.Address ?? string.Empty;
                starter.Phone = profile.PhoneNo ?? string.Empty;

                if (isBride)
                {
                    model.BrideMaritalStatus = profile.MaritalStatus ?? string.Empty;
                }
            }
        }

        return View(model);
    }

    // POST: New Application
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewApplicationViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var isGroomFirst = string.Equals(
            model.StartingParty, "Groom", StringComparison.OrdinalIgnoreCase);

        var form = new MarriageApplicationForm
        {
            ProposedNikahDate = model.ProposedNikahDate,
            Venue = model.Venue,

            BrideMembershipNo = model.Bride.MembershipNo.Trim(),
            BrideName = model.Bride.Name.Trim(),
            BrideDateOfBirth = model.Bride.DateOfBirth,
            BrideResidentOf = model.Bride.ResidentOf,
            BrideGenotype = model.Bride.Genotype,
            BrideBloodGroup = model.Bride.BloodGroup,
            BrideMaritalStatus = model.BrideMaritalStatus,
            BrideProposedDowerAmount = model.BrideProposedDowerAmount,
            BrideDowerAmountReceivedInCash = model.BrideDowerAmountReceivedInCash,
            BrideSignatureTel = model.Bride.Phone.Trim(),

            BridegroomMembershipNo = model.Bridegroom.MembershipNo.Trim(),
            BridegroomName = model.Bridegroom.Name.Trim(),
            BridegroomDateOfBirth = model.Bridegroom.DateOfBirth,
            BridegroomResidentOf = model.Bridegroom.ResidentOf,
            BridegroomGenotype = model.Bridegroom.Genotype,
            BridegroomBloodGroup = model.Bridegroom.BloodGroup,
            BridegroomDowerAmountPaidInCash = model.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = model.BridegroomDowerAmountToBePaid,
            IsFirstNikah = model.IsFirstNikah,
            IsSecondThirdOrFourthNikah = model.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = model.FormerWifeIsDead,
            HasDivorcedFormerWife = model.HasDivorcedFormerWife,
            FormerWifeIsPresent = model.FormerWifeIsPresent,
            FormerWifeObtainedKhula = model.FormerWifeObtainedKhula,
            BridegroomSignatureTel = model.Bridegroom.Phone.Trim(),

            ApplicationStage = ApplicationStage.ApplicantsReview,

            // Whoever started the form has already filled their section; the
            // form is now waiting on the partner (either party can be first).
            FormStage = isGroomFirst
                ? MarriageFormStage.AwaitingBride
                : MarriageFormStage.AwaitingBridegroom
        };

        var created = await _formService.StartApplicationAsync(form, ct);

        TempData["Success"] =
            $"Application {created.ReferenceNumber} started. Your partner can sign in and continue it from their dashboard.";

        return RedirectToAction("Index", "Applications");
    }

    private static bool IsFemale(string? sex) =>
        !string.IsNullOrWhiteSpace(sex) &&
        (sex.Equals("female", StringComparison.OrdinalIgnoreCase) ||
         sex.Equals("f", StringComparison.OrdinalIgnoreCase) ||
         sex.Contains("female", StringComparison.OrdinalIgnoreCase));

    private string? GetCurrentMembershipNo() =>
        User.FindFirstValue(ClaimNames.MembershipNo)
        ?? User.FindFirstValue(ClaimTypes.Name);
}
