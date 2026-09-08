using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

// Lookup exposes member PII (names, phones, addresses) so it must not be
// reachable anonymously.
[Authorize]
[ApiController]
[Route("api/members")]
public class MemberLookupController : ControllerBase
{
    private readonly IMemberLookupService _lookup;
    private readonly IPartnerEligibilityService _eligibility;

    public MemberLookupController(IMemberLookupService lookup, IPartnerEligibilityService eligibility)
    {
        _lookup = lookup;
        _eligibility = eligibility;
    }

    [HttpGet("lookup/{chandaNo}")]
    public async Task<IActionResult> Lookup(string chandaNo, CancellationToken ct)
    {
        var dto = await _lookup.LookupAsync(chandaNo, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("partner-eligibility/{chandaNo}")]
    public async Task<IActionResult> PartnerEligibility(string chandaNo, CancellationToken ct)
    {
        var dto = await _lookup.LookupAsync(chandaNo, ct);
        if (dto is null)
            return NotFound();

        var result = await _eligibility.ValidateCreateAsync(chandaNo, partnerIsGroom: false, ct);
        return Ok(new { member = dto, blocked = !result.IsAllowed, message = result.Message });
    }
}