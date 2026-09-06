
using System;
using Application.Interfaces;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Presentation.Requests;


namespace Presentation.Controllers;

[ApiController]
[Route("api/marriage-forms")]
[Authorize(Policy = "StageVerifier")]
public class MarriageFormsController : Controller
{
    private readonly IMarriageApplicationFormService _formService;
    public MarriageFormsController(IMarriageApplicationFormService formService)
    {
        _formService = formService;
    }

    // GET api/marriage-forms/{id}
    [HttpGet]

    // POST api/marriage-forms/{formId}/revert
    [HttpPost("{formId:guid}/revert")]
    public async Task<IActionResult> RevertStage(
        Guid formId,
        [FromBody] RevertStageRequest request,
        CancellationToken cancellationToken)
    {
        var membershipNo = GetCurrentMembershipNo();
        if (string.IsNullOrWhiteSpace(membershipNo))
            return Unauthorized();

        var result = await _formService.RevertStageAsync(
            formId, request.TargetStage, request.Reason, membershipNo, cancellationToken);

        return result switch
        {
            RevertStageResult.Success => NoContent(),
            RevertStageResult.FormNotFound => NotFound(),
            RevertStageResult.InvalidTargetStage => BadRequest(new { message = "Cannot revert to that stage from the current state." }),
            RevertStageResult.Unauthorized => Forbid(),
            _ => StatusCode(500)
        };
    }

    private string? GetCurrentMembershipNo()
    {
        return User.FindFirstValue(ClaimNames.MembershipNo)
            ?? User.FindFirstValue(ClaimTypes.Name);
    }
}