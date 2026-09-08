using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/members")]
public class MemberLookupController : ControllerBase
{
    private readonly IMemberLookupService _lookup;

    public MemberLookupController(IMemberLookupService lookup)
    {
        _lookup = lookup;
    }

    [HttpGet("lookup/{chandaNo}")]
    public async Task<IActionResult> Lookup(string chandaNo, CancellationToken ct)
    {
        var dto = await _lookup.LookupAsync(chandaNo, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
}
