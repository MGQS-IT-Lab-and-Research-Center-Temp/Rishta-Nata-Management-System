using Application.Interfaces;
using Application.Interfaces.Service;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarriageApplicationFormsController : ControllerBase
{
    private readonly IMarriageApplicationFormService _marriageApplicationFormService;

    public MarriageApplicationFormsController(
        IMarriageApplicationFormService marriageApplicationFormService)
    {
        _marriageApplicationFormService = marriageApplicationFormService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(MarriageApplicationForm model)
    {
        var result = await _marriageApplicationFormService.CreateAsync(model);

        return Ok(result);
    }
}