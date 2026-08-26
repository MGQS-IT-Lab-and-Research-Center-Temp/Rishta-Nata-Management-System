
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.DTOs.MarriageApplicationFormDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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
        [FromBody] RevertStageRequestDto request,
        CancellationToken cancellationToken)
    {
        var verifierId = GetCurrentUserId();
        if (verifierId is null)
            return Unauthorized();

        var result = await _formService.RevertStageAsync(
            formId, request.TargetStage, request.Reason, verifierId.Value, cancellationToken);

        return result switch
        {
            RevertStageResult.Success => NoContent(),
            RevertStageResult.FormNotFound => NotFound(),
            RevertStageResult.InvalidTargetStage => BadRequest(new { message = "Cannot revert to that stage from the current state." }),
            RevertStageResult.Unauthorized => Forbid(),
            _ => StatusCode(500)
        };
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }
}