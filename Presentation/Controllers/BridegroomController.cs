using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using Domain.Entities;

public class BridegroomController : Controller
{
    private readonly IMarriageApplicationFormService _marriageApplicationFormService;

    public BridegroomController(IMarriageApplicationFormService marriageApplicationFormService)
    {
        _marriageApplicationFormService = marriageApplicationFormService;
    }

    // GET: Show empty form
    [HttpGet]
    public IActionResult Create()
    {
        var model = new BridegroomFormViewModel
        {
            ReferenceNumber = string.Empty // initialize safely
        };
        return View(model);
    }

    // POST: Handle form submission
    [HttpPost]
    public async Task<IActionResult> Create(BridegroomFormViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);


        // Check if groom already exists by membership number
        var form = await _marriageApplicationFormService.GetByMembershipNoAsync(model.BridegroomMembershipNo, ct);

        if (form == null)
        {
            // Groom is first to apply → generate reference number
            form = new MarriageApplicationForm
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
                FormerWifeObtainedKhula = model.FormerWifeObtainedKhula,
                ReferenceNumber = $"RN-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}"
            };

            await _marriageApplicationFormService.CreateAsync(form, ct);
        }
        else
        {
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

            await _marriageApplicationFormService.UpdateAsync(form, ct);
        }

        return RedirectToAction("Confirmation", new { id = form.MarriageApplicationId });
    }
    

    // GET: Confirmation page
    [HttpGet]
    public async Task<IActionResult> Confirmation(Guid id, CancellationToken ct)
    {
        var form = await _marriageApplicationFormService.GetByIdAsync(id, ct);
        if (form == null) return NotFound();

        return View(form);
    }

    // GET: Detail page
    [HttpGet]
    public async Task<IActionResult> Detail(Guid id, CancellationToken ct)
    {
        var form = await _marriageApplicationFormService.GetByIdAsync(id, ct);
        if (form == null) return NotFound();

        return View(form);
    }
    
}
