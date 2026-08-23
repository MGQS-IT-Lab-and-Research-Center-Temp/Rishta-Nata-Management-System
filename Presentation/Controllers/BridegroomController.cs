using Application.Interfaces;
using Application.Interfaces.Auth;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

public class BridegroomController : Controller
{
    private readonly IBridegroomService _bridegroomService;
    private readonly IStageAuthorizationService _stageAuthorizationService;
    public BridegroomController(IBridegroomService bridegroomService, IStageAuthorizationService stageAuthorizationService)
    {
        _bridegroomService = bridegroomService;
        _stageAuthorizationService = stageAuthorizationService;
    }

    // GET: Show empty form
    [HttpGet]
    public IActionResult Create()
    {
        return View(new BridegroomFormViewModel());
    }

    // POST: Handle form submission
    [HttpPost]
    public async Task<IActionResult> Create(BridegroomFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);
        var authResult = await _stageAuthorizationService.AuthorizeAsync(
            model.MarriageApplicationFormId,
            MarriageFormStage.AwaitingBridegroom,
            User,
            ct);

        if (!authResult.IsAuthorized)
            return Forbid(); 
        var bridegroom = new BridegroomFormSection
        {
            
            BridegroomName = model.BridegroomName,
            BridegroomMembershipNo = model.BridegroomMembershipNo,
            BridegroomDateOfBirth = model.BridegroomDateOfBirth,
            BridegroomResidentOf = model.BridegroomResidentOf,
            BridegroomGenotype = model.BridegroomGenotype,
            BridegroomBloodGroup = model.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash = model.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = model.BridegroomDowerAmountToBePaid,
            BridegroomSignatureTel = model.BridegroomPhoneNumber,
            IsFirstNikah = model.IsFirstNikah,
            IsSecondThirdOrFourthNikah = model.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = model.FormerWifeIsDead,
            HasDivorcedFormerWife = model.HasDivorcedFormerWife,
            FormerWifeIsPresent = model.FormerWifeIsPresent,
            FormerWifeObtainedKhula = model.FormerWifeObtainedKhula
        };

        var savedBridegroom = await _bridegroomService.CreateAsync(bridegroom, ct);
        await _stageAuthorizationService.AdvanceStageAsync(model.MarriageApplicationFormId, MarriageFormStage.AwaitingWitnesses,
ct);
        return RedirectToAction(nameof(Confirmation), new { id = savedBridegroom.Id });

    }



    // GET: Confirmation page
    [HttpGet]
    public async Task<IActionResult> Confirmation(Guid id)
    {
        var application = await _bridegroomService.GetByIdAsync(id);
        if (application == null)
            return NotFound();

        return View(application);
    }


    // GET: Detail page
    [HttpGet]
    public async Task<IActionResult> Detail(Guid id, CancellationToken ct)
    {
        var form = await _bridegroomService.GetByIdAsync(id, ct);
        if (form == null) return NotFound();

        return View(form);
    }

}
