using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel;

namespace Presentation.Controllers;

[Route("Witness")]
public class WitnessController : Controller
{
    private readonly IMarriageApplicationFormService _applicationService;

    public WitnessController(IMarriageApplicationFormService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("Create/{marriageApplicationId:guid}")]
    public async Task<IActionResult> Create(Guid marriageApplicationId)
    {
        var application = await _applicationService.GetByMarriageApplicationIdAsync(
            marriageApplicationId);

        if (application is null)
            return NotFound("The marriage application was not found.");

        return View(ToViewModel(application));
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(WitnessViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        ValidateWitnesses(model);

        if (!ModelState.IsValid)
            return View(model);

        return View("Confirm", model);
    }

    [HttpPost("Confirm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(
        WitnessViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var application = await _applicationService.GetByMarriageApplicationIdAsync(
            model.MarriageApplicationId);

        if (application is null || application.ReferenceNumber != model.ReferenceNumber)
            return NotFound("The marriage application was not found.");

        application.WitnessOneMembershipNo = model.WitnessOneMembershipNo;
        application.WitnessOneName = model.WitnessOneName;
        application.WitnessOneAddress = model.WitnessOneAddress;
        application.WitnessOneTel = model.WitnessOneTel;
        application.WitnessOneSignatureDate = model.WitnessOneSignatureDate;

        application.WitnessTwoMembershipNo = model.WitnessTwoMembershipNo;
        application.WitnessTwoName = model.WitnessTwoName;
        application.WitnessTwoAddress = model.WitnessTwoAddress;
        application.WitnessTwoTel = model.WitnessTwoTel;
        application.WitnessTwoSignatureDate = model.WitnessTwoSignatureDate;

        await _applicationService.UpdateAsync(application, cancellationToken);

        return RedirectToAction(
            nameof(Create),
            new { marriageApplicationId = model.MarriageApplicationId });
    }

    private void ValidateWitnesses(WitnessViewModel model)
    {
        if (model.WitnessOneIsMember &&
            string.IsNullOrWhiteSpace(model.WitnessOneMembershipNo))
        {
            ModelState.AddModelError(
                nameof(model.WitnessOneMembershipNo),
                "Membership number is required when witness 1 is a member.");
        }

        if (!model.WitnessOneIsMember)
        {
            if (string.IsNullOrWhiteSpace(model.WitnessOneName))
                ModelState.AddModelError(nameof(model.WitnessOneName), "Witness 1 name is required.");
            if (string.IsNullOrWhiteSpace(model.WitnessOneAddress))
                ModelState.AddModelError(nameof(model.WitnessOneAddress), "Witness 1 address is required.");
            if (string.IsNullOrWhiteSpace(model.WitnessOneTel))
                ModelState.AddModelError(nameof(model.WitnessOneTel), "Witness 1 telephone is required.");
        }

        if (model.WitnessTwoIsMember &&
            string.IsNullOrWhiteSpace(model.WitnessTwoMembershipNo))
        {
            ModelState.AddModelError(
                nameof(model.WitnessTwoMembershipNo),
                "Membership number is required when witness 2 is a member.");
        }

        if (!model.WitnessTwoIsMember)
        {
            if (string.IsNullOrWhiteSpace(model.WitnessTwoName))
                ModelState.AddModelError(nameof(model.WitnessTwoName), "Witness 2 name is required.");
            if (string.IsNullOrWhiteSpace(model.WitnessTwoAddress))
                ModelState.AddModelError(nameof(model.WitnessTwoAddress), "Witness 2 address is required.");
            if (string.IsNullOrWhiteSpace(model.WitnessTwoTel))
                ModelState.AddModelError(nameof(model.WitnessTwoTel), "Witness 2 telephone is required.");
        }
    }

    private static WitnessViewModel ToViewModel(MarriageApplicationForm application) =>
        new WitnessViewModel
        {
            MarriageApplicationId = application.MarriageApplicationId,
            ReferenceNumber = application.ReferenceNumber,
            BrideName = application.BrideName,
            BridegroomName = application.BridegroomName,
            WitnessOneMembershipNo = application.WitnessOneMembershipNo,
            WitnessOneName = application.WitnessOneName,
            WitnessOneAddress = application.WitnessOneAddress,
            WitnessOneTel = application.WitnessOneTel,
            WitnessOneSignatureDate = application.WitnessOneSignatureDate,
            WitnessTwoMembershipNo = application.WitnessTwoMembershipNo,
            WitnessTwoName = application.WitnessTwoName,
            WitnessTwoAddress = application.WitnessTwoAddress,
            WitnessTwoTel = application.WitnessTwoTel,
            WitnessTwoSignatureDate = application.WitnessTwoSignatureDate
        };
}
