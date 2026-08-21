using Application.Interfaces;
using Application.Mappings;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel;

namespace Presentation.Controllers;

public class MarriageApplicationFormController : Controller
{
    private readonly IMarriageApplicationFormService
        _marriageApplicationFormService;

    public MarriageApplicationFormController(
        IMarriageApplicationFormService marriageApplicationFormService)
    {
        _marriageApplicationFormService =
            marriageApplicationFormService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new MarriageApplicationFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        MarriageApplicationFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // ViewModel → DTO
            var dto = MarriageApplicationFormMapping.ToDto(model);

            // DTO → Entity
            var entity = MarriageApplicationFormMapping.ToEntity(dto);

            // Send Entity to Service
            var created =
                await _marriageApplicationFormService.CreateAsync(
                    entity,
                    cancellationToken);

            return RedirectToAction(
                nameof(Success),
                new { id = created.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Success(Guid id)
    {
        ViewBag.MarriageApplicationFormId = id;

        return View();
    }
}