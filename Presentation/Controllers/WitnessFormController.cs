using Application.Interfaces;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel;

namespace Presentation.Controllers;

public class WitnessFormController : Controller
{
    private readonly IWitnessFormService _witnessFormService;

    public WitnessFormController(
        IWitnessFormService witnessFormService)
    {
        _witnessFormService = witnessFormService;
    }

    [HttpGet]
    public async Task<IActionResult> Complete(
        string token,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("Invalid witness link.");
        }

        var witness = await _witnessFormService
            .GetWitnessByTokenAsync(token, cancellationToken);

        if (witness is null)
        {
            return NotFound("Witness link not found.");
        }

        if (witness.IsCompleted)
        {
            return View("AlreadyCompleted");
        }

        var model = new WitnessViewModel
        {
            Id = witness.Id,
            FullName = witness.FullName,
            Email = witness.Email,
            PhoneNumber = witness.PhoneNumber,
            SignatureDate = witness.SignatureDate ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(
        WitnessViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var witnessDto = new WitnessDto
        {
            Id = model.Id,
            FullName = model.FullName ?? string.Empty,
            Email = model.Email ?? string.Empty,
            PhoneNumber = model.PhoneNumber ?? string.Empty,
            SignatureDate = model.SignatureDate ?? string.Empty
        };

        var existingWitness =
            await _witnessFormService
                .GetWitnessByIdAsync(
                    witnessDto.Id,
                    cancellationToken);

        if (existingWitness is null)
        {
            return NotFound("Witness could not be found.");
        }

        if (existingWitness.IsCompleted)
        {
            return View("AlreadyCompleted");
        }

        var completed =
            await _witnessFormService
                .CompleteWitnessAsync(
                    witnessDto,
                    cancellationToken);

        if (!completed)
        {
            return BadRequest(
                "Unable to complete witness information.");
        }

        return RedirectToAction(nameof(Success));
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}