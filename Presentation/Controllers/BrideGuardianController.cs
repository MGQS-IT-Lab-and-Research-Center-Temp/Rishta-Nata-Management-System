using Application.Interfaces;
using Infrastructure.Mapper;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping;
using Presentation.ViewModel;

namespace Presentation.Controllers;

[Route("BrideGuardian")]
public class BrideGuardianController : Controller
{
    private readonly IMarriageApplicationFormService _applicationService;
    private readonly IBrideGuardianService _guardianService;

    public BrideGuardianController(
        IMarriageApplicationFormService applicationService,
        IBrideGuardianService guardianService)
    {
        _applicationService = applicationService;
        _guardianService = guardianService;
    }

    [HttpGet("Create/{marriageApplicationId:guid}")]
    public async Task<IActionResult> Create(
        Guid marriageApplicationId,
        CancellationToken cancellationToken)
    {
        var application = await _applicationService.GetByMarriageApplicationIdAsync(
            marriageApplicationId);

        if (application is null)
        {
            return NotFound("The marriage application was not found.");
        }

        return View(BrideGuardianViewModelMapper.ToViewModel(
            new MarriageApplicationFormViewModel
            {
                MarriageApplicationId = application.MarriageApplicationId,
                ReferenceNumber = application.ReferenceNumber,
                BrideName = application.BrideName,
                BrideFatherName = application.BrideFatherName,
                BrideFatherMembershipNo = application.BrideFatherMembershipNo,
                BrideDateOfBirth = application.BrideDateOfBirth,
                BrideResidentOf = application.BrideResidentOf,
                BrideGenotype = application.BrideGenotype,
                BrideBloodGroup = application.BrideBloodGroup,
                BrideMaritalStatus = application.BrideMaritalStatus,
                BrideProposedDowerAmount = application.BrideProposedDowerAmount,
                BrideDowerAmountReceivedInCash = application.BrideDowerAmountReceivedInCash,
                BridegroomName = application.BridegroomName,
                BridegroomFatherName = application.BridegroomFatherName,
                BridegroomFatherMembershipNo = application.BridegroomFatherMembershipNo,
                BridegroomDateOfBirth = application.BridegroomDateOfBirth,
                BridegroomResidentOf = application.BridegroomResidentOf
            },
            application.ReferenceNumber));
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BrideGuardianViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ValidateFathers(model);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Do NOT redirect here.
        // We need to preserve the model for the confirmation page.
        return View("Confirm", model);
    }

    private void ValidateFathers(BrideGuardianViewModel model)
    {
        if (model.BrideFatherIsMember && string.IsNullOrWhiteSpace(model.BrideFatherMembershipNo))
        {
            ModelState.AddModelError(
                nameof(model.BrideFatherMembershipNo),
                "Membership number is required when the bride's father is a member.");
        }

        if (!model.BrideFatherIsMember && string.IsNullOrWhiteSpace(model.BrideFatherName))
        {
            ModelState.AddModelError(
                nameof(model.BrideFatherName),
                "The bride's father's name is required.");
        }

        if (model.BridegroomFatherIsMember && string.IsNullOrWhiteSpace(model.BridegroomFatherMembershipNo))
        {
            ModelState.AddModelError(
                nameof(model.BridegroomFatherMembershipNo),
                "Membership number is required when the bridegroom's father is a member.");
        }

        if (!model.BridegroomFatherIsMember && string.IsNullOrWhiteSpace(model.BridegroomFatherName))
        {
            ModelState.AddModelError(
                nameof(model.BridegroomFatherName),
                "The bridegroom's father's name is required.");
        }
    }


    // POST: guardian confirmed — now persist
    [HttpPost("Confirm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(BrideGuardianViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var application = await _applicationService.GetByMarriageApplicationIdAsync(
            model.MarriageApplicationId);

        if (application is null || application.ReferenceNumber != model.ReferenceNumber)
        {
            return NotFound("The marriage application was not found.");
        }

        var guardian = BrideGuardianMapper.ToEntity(
            BrideGuardianViewModelMapper.ToDto(model));

        await _guardianService.CreateAsync(guardian, cancellationToken);

        application.GuardianName = model.GuardianName;
        application.GuardianRelationToBride = model.GuardianRelationToBride;
        application.GuardianAddress = model.GuardianAddress;
        application.GuardianTel = model.GuardianTel;
        application.GuardianSignatureDate = model.GuardianSignatureDate;

        application.BrideFatherName = model.BrideFatherName;
        application.BrideFatherMembershipNo = model.BrideFatherMembershipNo;
        application.BridegroomFatherName = model.BridegroomFatherName;
        application.BridegroomFatherMembershipNo = model.BridegroomFatherMembershipNo;

        await _applicationService.UpdateAsync(application, cancellationToken);

        return RedirectToAction(nameof(Details), new { id = guardian.BrideGuardianId });
    }

    

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var guardian = await _guardianService.GetByIdAsync(id, cancellationToken);

        return guardian is null
            ? NotFound()
            : View(BrideGuardianViewModelMapper.ToDetailsViewModel(
                BrideGuardianMapper.ToDto(guardian)));
    }

}

