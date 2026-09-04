using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Mapper;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mapping.Bridegroom;
using Presentation.ViewModels;

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

        var dto = BridegroomMapping.ToDto(model);
        var bridegroom = BrideGroomMapper.ToEntity(dto);

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
