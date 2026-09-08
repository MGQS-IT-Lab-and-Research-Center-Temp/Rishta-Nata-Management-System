using Application.Interfaces;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Presentation.Requests;


namespace Presentation.Controllers;

[Authorize(Policy = "StageVerifier")]
[Route("api/marriage-forms")]
public class MarriageFormController : Controller
{
    private readonly IMarriageApplicationFormService _formService;
    public MarriageFormController(IMarriageApplicationFormService formService)
    {
        _formService = formService;
    }

    // POST api/marriage-forms/{formId}/revert
    [HttpPost("{formId:guid}/revert")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RevertStage(
        Guid formId,
        [FromBody] RevertStageRequest request,
        CancellationToken cancellationToken)
    {
        var membershipNo = GetCurrentMembershipNo();
        if (string.IsNullOrWhiteSpace(membershipNo))
            return Unauthorized();

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();

            return BadRequest(new
            {
                message = errors.Count > 0
                    ? string.Join(" ", errors)
                    : "A valid target stage and rejection reason are required."
            });
        }

        var result = await _formService.RevertStageAsync(
            formId, request.TargetStage!.Value, request.Reason, membershipNo, cancellationToken);

        return result switch
        {
            RevertStageResult.Success => NoContent(),
            RevertStageResult.FormNotFound => NotFound(),
            RevertStageResult.InvalidTargetStage => BadRequest(new { message = "Cannot revert to that stage from the current state." }),
            RevertStageResult.ApplicationAlreadyApproved => BadRequest(new { message = "The application has already been approved and cannot be reverted." }),
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