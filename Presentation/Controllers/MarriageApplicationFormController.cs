using Application.Interfaces;
using Application.Interfaces.Service;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class MarriageApplicationFormController : Controller
{
    private readonly IMarriageApplicationFormService _marriageApplicationFormService;

    public MarriageApplicationFormController(
        IMarriageApplicationFormService marriageApplicationFormService)
    {
        _marriageApplicationFormService = marriageApplicationFormService;
    }

    // GET: /MarriageApplicationForm/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /MarriageApplicationForm/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MarriageApplicationForm model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _marriageApplicationFormService.CreateAsync(model);

        return RedirectToAction("Success");
    }

    // GET: /MarriageApplicationForm/Success
    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}