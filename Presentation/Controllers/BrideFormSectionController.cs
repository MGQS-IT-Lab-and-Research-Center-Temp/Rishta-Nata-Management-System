using Application.Interfaces.Service;
using Infrastructure.DTOs.Bride;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BridesController : ControllerBase
{
    private readonly IBrideFormSectionService _brideService;

    public BridesController(IBrideFormSectionService brideService)
    {
        _brideService = brideService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBrideFormSectionDto dto)
    {
        var result = await _brideService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var bride = await _brideService.GetByIdAsync(id);

        if (bride == null)
            return NotFound();

        return Ok(bride);
    }

    [HttpGet("application/{marriageApplicationFormId}")]
    public async Task<IActionResult> GetByApplication(Guid marriageApplicationFormId)
    {
        var bride = await _brideService.GetByMarriageApplicationFormIdAsync(marriageApplicationFormId);

        if (bride == null)
            return NotFound();

        return Ok(bride);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateBrideFormSectionDto dto)
    {
        await _brideService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _brideService.DeleteAsync(id);
        return NoContent();
    }
}
