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

        // Check if groom already exists
        var form = await _bridegroomService
            .GetByMembershipNoAsync(model.BridegroomMembershipNo, ct);

        if (form == null)
        {
            // Groom is first to apply → create new application
            var newForm = new BrideGroom
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

            var application = await _bridegroomService.CreateAsync(newForm, ct);

            return RedirectToAction("Confirmation", new { id = application.Id });

        }

        // Groom already exists → update details
        form.BridegroomName = model.BridegroomName;
        form.BridegroomDateOfBirth = model.BridegroomDateOfBirth;
        form.BridegroomResidentOf = model.BridegroomResidentOf;
        form.BridegroomGenotype = model.BridegroomGenotype;
        form.BridegroomBloodGroup = model.BridegroomBloodGroup;
        form.BridegroomDowerAmountPaidInCash = model.BridegroomDowerAmountPaidInCash;
        form.BridegroomDowerAmountToBePaid = model.BridegroomDowerAmountToBePaid;
        form.BridegroomSignatureTel = model.BridegroomPhoneNumber;
        form.IsFirstNikah = model.IsFirstNikah;
        form.IsSecondThirdOrFourthNikah = model.IsSecondThirdOrFourthNikah;
        form.FormerWifeIsDead = model.FormerWifeIsDead;
        form.HasDivorcedFormerWife = model.HasDivorcedFormerWife;
        form.FormerWifeIsPresent = model.FormerWifeIsPresent;
        form.FormerWifeObtainedKhula = model.FormerWifeObtainedKhula;

        await _bridegroomService.UpdateAsync(form, ct);

        return RedirectToAction("Confirmation", new { id = form.Id });
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
