
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

    /*

    [HttpGet("Create/{referenceNumber}")]
    public async Task<IActionResult> Create(string referenceNumber, CancellationToken cancellationToken)
    {
        var application = await _applicationService.GetByReferenceNumberAsync(
            referenceNumber,
            cancellationToken);

        if (application is null)
        {
            return NotFound("The marriage application was not found.");
        }

        var existingGuardian = await _guardianService.GetByMarriageApplicationIdAsync(
            application.MarriageApplicationId,
            cancellationToken);

        if (existingGuardian is not null)
        {
            return Conflict("A guardian has already been recorded for this application.");
        }

        return View(BrideGuardianViewModelMapper.ToViewModel(
            new MarriageApplicationFormViewModel
            {
                MarriageApplicationId = application.MarriageApplicationId,
                BrideName = application.BrideName,
                BrideFatherName = application.BrideFatherName,
                BrideDateOfBirth = application.BrideDateOfBirth,
                BrideResidentOf = application.BrideResidentOf,
                BrideGenotype = application.BrideGenotype,
                BrideBloodGroup = application.BrideBloodGroup,
                BrideMaritalStatus = application.BrideMaritalStatus,
                BrideProposedDowerAmount = application.BrideProposedDowerAmount,
                BrideDowerAmountReceivedInCash = application.BrideDowerAmountReceivedInCash,
                BridegroomName = application.BridegroomName,
                BridegroomFatherName = application.BridegroomFatherName,
                BridegroomDateOfBirth = application.BridegroomDateOfBirth,
                BridegroomResidentOf = application.BridegroomResidentOf
            },
            application.ReferenceNumber));
    }

    */
    [HttpPost("Create/{referenceNumber}")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BrideGuardianViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Do NOT redirect here.
        // We need to preserve the model for the confirmation page.
        return View("Confirm", model);
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

