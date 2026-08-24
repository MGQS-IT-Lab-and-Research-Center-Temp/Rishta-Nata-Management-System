using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using Domain.Entities;

public class BridegroomController : Controller
{
    private readonly IBridegroomService _bridegroomService;

    public BridegroomController(IBridegroomService bridegroomService)
    {
        _bridegroomService = bridegroomService;
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
