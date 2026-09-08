using System.Security.Claims;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;

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
    public async Task<IActionResult> PartnerEligibility(string chandaNo, CancellationToken ct, bool partnerIsGroom = false)
    {
        var dto = await _lookup.LookupAsync(chandaNo, ct);
        if (dto is null)
            return NotFound();

        if (IsCurrentUser(dto.ChandaNo))
        {
            return Ok(new { member = dto, blocked = true, message = PartnerMessages.CannotBeYourself });
        }

        var result = await _eligibility.ValidateCreateAsync(chandaNo, partnerIsGroom, ct);
        return Ok(new { member = dto, blocked = !result.IsAllowed, message = result.Message });
    }

    private bool IsCurrentUser(string chandaNo)
    {
        var currentMembershipNo = User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name);

        return !string.IsNullOrWhiteSpace(currentMembershipNo) &&
               string.Equals(
                   chandaNo?.Trim(),
                   currentMembershipNo.Trim(),
                   StringComparison.OrdinalIgnoreCase);
    }
}