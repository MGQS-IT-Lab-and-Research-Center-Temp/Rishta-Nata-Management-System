using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Authorization;
using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Mapping;
using Presentation.ViewModels;

namespace Presentation.Controllers;

[Authorize]
public class MarriageApplicationController : Controller
{
    private readonly IMarriageApplicationFormService _formService;
    private readonly IMemberDashboardService _memberDashboardService;
    private readonly IBrideSectionService _brideSectionService;
    private readonly IBridegroomSectionService _bridegroomSectionService;
    private readonly IMemberLookupService _memberLookupService;
    private readonly IPartnerEligibilityService _eligibility;

    public MarriageApplicationController(
        IMarriageApplicationFormService formService,
        IMemberDashboardService memberDashboardService,
        IBrideSectionService brideSectionService,
        IBridegroomSectionService bridegroomSectionService,
        IMemberLookupService memberLookupService,
        IPartnerEligibilityService eligibility)
    {
        _formService = formService;
        _memberDashboardService = memberDashboardService;
        _brideSectionService = brideSectionService;
        _bridegroomSectionService = bridegroomSectionService;
        _memberLookupService = memberLookupService;
        _eligibility = eligibility;
    }

    // GET: New Application (role-aware — either party can start)
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var membershipNo = GetCurrentMembershipNo();

        var active = await _memberDashboardService.GetActiveApplicationAsync(
            membershipNo ?? string.Empty, ct);

        if (active is not null)
        {
            if (active.IsAwaitingYourSection)
            {
                TempData["Info"] = "You already have an application in progress — continue it below.";
                return RedirectToAction(nameof(Continue), new { id = active.Id });
            }

            TempData["Info"] = "You already have an application in progress.";
            return RedirectToAction("Index", "Applications");
        }

        var model = new NewApplicationViewModel();

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
        var isGroomFirst = string.Equals(
            model.StartingParty, "Groom", StringComparison.OrdinalIgnoreCase);

        // The partner's Name/DateOfBirth/ResidentOf/Phone are not rendered as editable
        // inputs on the starter-only view, so they are never posted; the shared
        // ApplicantPartyInfo [Required] annotations would otherwise fail validation for
        // the partner's unposted fields. The partner's identity is derived server-side
        // from the Tajneed lookup, so those errors are dropped here.
        var partnerPrefix = isGroomFirst ? "Bride" : "Bridegroom";
        foreach (var prop in new[] { "Name", "DateOfBirth", "ResidentOf", "Phone" })
        {
            ModelState.Remove($"{partnerPrefix}.{prop}");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var starterMembershipNo = (isGroomFirst ? model.Bridegroom.MembershipNo : model.Bride.MembershipNo)?.Trim();
        var partnerMembershipNo = (isGroomFirst ? model.Bride.MembershipNo : model.Bridegroom.MembershipNo)?.Trim();

        if (string.Equals(starterMembershipNo, partnerMembershipNo, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError($"{partnerPrefix}.MembershipNo", PartnerMessages.CannotBeYourself);
            return View(model);
        }

        var eligibility = await _eligibility.ValidateCreateAsync(
            partnerMembershipNo ?? string.Empty,
            !isGroomFirst,
            ct);

        if (!eligibility.IsAllowed)
        {
            ModelState.AddModelError($"{partnerPrefix}.MembershipNo", eligibility.Message);
            return View(model);
        }

        var partner = await _memberLookupService.LookupAsync(partnerMembershipNo ?? string.Empty, ct);
        if (partner is null)
        {
            ModelState.AddModelError(string.Empty,
                "We couldn't find that membership number. Please check it and try again.");
            return View(model);
        }

        var currentMembershipNo = GetCurrentMembershipNo()?.Trim();
        if (!string.IsNullOrWhiteSpace(currentMembershipNo) &&
            string.Equals(partner.ChandaNo, currentMembershipNo, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError($"{partnerPrefix}.MembershipNo", PartnerMessages.CannotBeYourself);
            return View(model);
        }

        var partnerPhone = partner.PhoneNo;
        var form = new MarriageApplicationForm
        {
            ProposedNikahDate = model.ProposedNikahDate,
            Venue = model.Venue,

            BrideMembershipNo = (isGroomFirst ? partner.ChandaNo : model.Bride.MembershipNo.Trim()),
            BrideName = (isGroomFirst ? partner.FullName : model.Bride.Name.Trim()),
            BrideDateOfBirth = (isGroomFirst ? (partner.DateOfBirth ?? model.Bride.DateOfBirth) : model.Bride.DateOfBirth),
            BrideResidentOf = (isGroomFirst ? partner.Address : model.Bride.ResidentOf),
            BrideGenotype = model.Bride.Genotype,
            BrideBloodGroup = model.Bride.BloodGroup,
            BrideMaritalStatus = model.BrideMaritalStatus,
            BrideDivorceEvidence = model.BrideDivorceEvidence,
            BrideProposedDowerAmount = model.BrideProposedDowerAmount,
            BrideDowerAmountReceivedInCash = model.BrideDowerAmountReceivedInCash,
            BrideSignatureTel = (isGroomFirst ? partnerPhone : model.Bride.Phone.Trim()),

            BridegroomMembershipNo = (isGroomFirst ? model.Bridegroom.MembershipNo.Trim() : partner.ChandaNo),
            BridegroomName = (isGroomFirst ? model.Bridegroom.Name.Trim() : partner.FullName),
            BridegroomDateOfBirth = (isGroomFirst ? model.Bridegroom.DateOfBirth : (partner.DateOfBirth ?? model.Bridegroom.DateOfBirth)),
            BridegroomResidentOf = (isGroomFirst ? model.Bridegroom.ResidentOf : partner.Address),
            BridegroomGenotype = model.Bridegroom.Genotype,
            BridegroomBloodGroup = model.Bridegroom.BloodGroup,
            BridegroomDowerAmountPaidInCash = model.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = model.BridegroomDowerAmountToBePaid,
            IsFirstNikah = model.IsFirstNikah,
            IsSecondThirdOrFourthNikah = model.IsSecondThirdOrFourthNikah ?? false,
            FormerWifeIsDead = model.FormerWifeIsDead ?? false,
            HasDivorcedFormerWife = model.HasDivorcedFormerWife ?? false,
            BridegroomDivorceEvidence = model.BridegroomDivorceEvidence,
            FormerWifeIsPresent = model.FormerWifeIsPresent ?? false,
            FormerWifeObtainedKhula = model.FormerWifeObtainedKhula ?? false,
            BridegroomSignatureTel = (isGroomFirst ? model.Bridegroom.Phone.Trim() : partnerPhone),

            ApplicationStage = ApplicationStage.ApplicantsReview,
            FormStage = isGroomFirst
                ? MarriageFormStage.AwaitingBride
                : MarriageFormStage.AwaitingBridegroom
        };

        var created = await _formService.StartApplicationAsync(form, ct);

        TempData["Success"] =
            $"Application {created.ReferenceNumber} started. Your partner can sign in and continue it from their dashboard.";

        return RedirectToAction("Index", "Applications");
    }

    // GET: Continue (complete your section)
    [HttpGet]
    public async Task<IActionResult> Continue(Guid id, CancellationToken ct)
    {
        var form = await _formService.GetByIdAsync(id, ct);

        if (form is null)
        {
            return RedirectToAction("Index", "Applications");
        }

        var membershipNo = GetCurrentMembershipNo() ?? string.Empty;
        var isBride = MembershipNumbersMatch(membershipNo, form.BrideMembershipNo);
        var isGroom = MembershipNumbersMatch(membershipNo, form.BridegroomMembershipNo);

        if (!isBride && !isGroom)
        {
            return RedirectToAction("Index", "Applications");
        }

        var awaiting = isBride
            ? form.FormStage == MarriageFormStage.AwaitingBride ||
              form.FormStage == MarriageFormStage.AwaitingApplicants
            : form.FormStage == MarriageFormStage.AwaitingBridegroom ||
              form.FormStage == MarriageFormStage.AwaitingApplicants;

        if (!awaiting)
        {
            TempData["Info"] = "This application is no longer awaiting your section.";
            return RedirectToAction("Index", "Applications");
        }

        var model = isBride
            ? ToBrideViewModel(form)
            : ToBridegroomViewModel(form);

        return View(model);
    }

    // POST: Continue
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Continue(Guid id, ContinueApplicationViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var membershipNo = GetCurrentMembershipNo() ?? string.Empty;

        StageAuthorizationResult result;

        if (string.Equals(model.Party, "Bride", StringComparison.OrdinalIgnoreCase))
        {
            var dto = MarriageFormRequestMapping.ToBrideDto(model);
            result = await _brideSectionService.SubmitBrideSectionAsync(membershipNo, id, dto, ct);
        }
        else
        {
            var dto = MarriageFormRequestMapping.ToBridegroomDto(model);
            result = await _bridegroomSectionService.SubmitBridegroomSectionAsync(membershipNo, id, dto, ct);
        }

        if (!result.IsAllowed)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["Success"] = "Your section has been submitted.";
        return RedirectToAction("Index", "Applications");
    }

    private static ContinueApplicationViewModel ToBrideViewModel(MarriageApplicationForm form) => new()
    {
        Id = form.Id,
        ReferenceNumber = form.ReferenceNumber,
        Party = "Bride",
        MembershipNo = form.BrideMembershipNo,
        Name = form.BrideName,
        DateOfBirth = form.BrideDateOfBirth,
        ResidentOf = form.BrideResidentOf,
        Phone = form.BrideSignatureTel,
        Genotype = form.BrideGenotype,
        BloodGroup = form.BrideBloodGroup,
        MaritalStatus = form.BrideMaritalStatus,
        BrideDivorceEvidence = form.BrideDivorceEvidence,
        ProposedDowerAmount = form.BrideProposedDowerAmount,
        DowerAmountReceivedInCash = form.BrideDowerAmountReceivedInCash
    };

    private static ContinueApplicationViewModel ToBridegroomViewModel(MarriageApplicationForm form) => new()
    {
        Id = form.Id,
        ReferenceNumber = form.ReferenceNumber,
        Party = "Groom",
        MembershipNo = form.BridegroomMembershipNo,
        Name = form.BridegroomName,
        DateOfBirth = form.BridegroomDateOfBirth,
        ResidentOf = form.BridegroomResidentOf,
        Phone = form.BridegroomSignatureTel,
        Genotype = form.BridegroomGenotype,
        BloodGroup = form.BridegroomBloodGroup,
        DowerAmountPaidInCash = form.BridegroomDowerAmountPaidInCash,
        DowerAmountToBePaid = form.BridegroomDowerAmountToBePaid,
        IsFirstNikah = form.IsFirstNikah,
        IsSecondThirdOrFourthNikah = form.IsSecondThirdOrFourthNikah,
        FormerWifeIsDead = form.FormerWifeIsDead,
        HasDivorcedFormerWife = form.HasDivorcedFormerWife,
        BridegroomDivorceEvidence = form.BridegroomDivorceEvidence,
        FormerWifeIsPresent = form.FormerWifeIsPresent,
        FormerWifeObtainedKhula = form.FormerWifeObtainedKhula
    };

    private static bool IsFemale(string? sex) =>
        !string.IsNullOrWhiteSpace(sex) &&
        (sex.Equals("female", StringComparison.OrdinalIgnoreCase) ||
         sex.Equals("f", StringComparison.OrdinalIgnoreCase) ||
         sex.Contains("female", StringComparison.OrdinalIgnoreCase));

    private static bool MembershipNumbersMatch(string a, string b) =>
        string.Equals(a?.Trim(), b?.Trim(), StringComparison.OrdinalIgnoreCase);

    private string? GetCurrentMembershipNo() =>
        User.FindFirstValue(ClaimNames.MembershipNo)
        ?? User.FindFirstValue(ClaimTypes.Name);
}
