using Application.Interfaces;
using Application.Workflow;
using Infrastructure.DTOs;
using Infrastructure.DTOs.BrideGroom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/marriage-applications/{id}/form")]
public class MarriageApplicationFormController : ControllerBase
{
    private readonly IMarriageApplicationFormService _formService;
    private readonly IMarriageApplicationFormDetailService _formDetailService;
    private readonly IBrideGuardianService _brideGuardianService;
    private readonly IBridegroomService _bridegroomService;
    private readonly IMarriageFormWorkflowService _workflowService;

    public MarriageApplicationFormController(
        IMarriageApplicationFormService formService,
        IMarriageApplicationFormDetailService formDetailService,
        IBrideGuardianService brideGuardianService,
        IBridegroomService bridegroomService,
        IMarriageFormWorkflowService workflowService)
    {
        _formService = formService;
        _formDetailService = formDetailService;
        _brideGuardianService = brideGuardianService;
        _bridegroomService = bridegroomService;
        _workflowService = workflowService;
    }

    private Guid CurrentUserId =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
            ? userId
            : Guid.Empty;

    [HttpGet]
    public async Task<IActionResult> GetForm(Guid id, CancellationToken ct)
    {
        var detail = await _formDetailService.GetDetailAsync(id, ct);
        return detail is null ? NotFound() : Ok(detail);
    }

    [HttpPost("bride")]
    [Authorize(Policy = "CanFillBrideSection")]
    public async Task<IActionResult> SubmitBride(Guid id, [FromBody] BrideSectionDto dto, CancellationToken ct)
    {
        var result = await _brideGuardianService.SubmitBrideSectionAsync(CurrentUserId, id, dto, ct);
        return result.IsAllowed ? Ok() : StatusCode(403, result.Message);
    }

    [HttpPut("bridegroom")]
    [Authorize(Policy = "CanFillBridegroomSection")]
    public async Task<IActionResult> SubmitBridegroom(Guid id, [FromBody] BridegroomSectionDto dto, CancellationToken ct)
    {
        var result = await _bridegroomService.SubmitBridegroomSectionAsync(CurrentUserId, id, dto, ct);
        return result.IsAllowed ? Ok() : StatusCode(403, result.Message);
    }

    [HttpPut("guardian-or-wakeel")]
    [Authorize(Policy = "CanFillGuardianOrWakeelSection")]
    public IActionResult SubmitGuardianOrWakeel(Guid id)
    {
        // BLOCKED: guardian/wakeel submission (backlog D2) has not been implemented
        // by anyone on the team yet — confirmed via full-repo search. Route exists
        // per F2's AC; wire the real call once D2 lands.
        return StatusCode(501, "Guardian/Wakeel submission is not yet implemented (backlog D2).");
    }

    [HttpPut("witnesses")]
    [Authorize(Policy = "CanFillWitnessesSection")]
    public IActionResult SubmitWitnesses(Guid id)
    {
        // BLOCKED: same as above — witness submission (backlog D2) not yet implemented.
        return StatusCode(501, "Witness submission is not yet implemented (backlog D2).");
    }

    [HttpPut("imam-verification")]
    [Authorize(Policy = "CanFillImamVerificationSection")]
    public async Task<IActionResult> SubmitImamVerification(Guid id, [FromBody] ImamVerificationSubmission submission, CancellationToken ct)
    {
        var result = await _workflowService.SubmitImamVerificationAsync(CurrentUserId, id, submission, ct);
        return result.IsAllowed ? Ok() : StatusCode(403, result.Message);
    }

    [HttpPut("jamaat-president")]
    [Authorize(Policy = "CanFillJamaatPresidentSection")]

    public async Task<IActionResult> SubmitJamaatPresident(Guid id, [FromBody] JamaatPresidentVerificationSubmission submission, CancellationToken ct)
    {
        var result = await _workflowService.SubmitJamaatPresidentVerificationAsync(CurrentUserId, id, submission, ct);
        return result.IsAllowed ? Ok() : StatusCode(403, result.Message);
    }

    [HttpPut("rishtanata-recommendation")]
    [Authorize(Policy = "CanFillRishtanataSection")]
    public async Task<IActionResult> SubmitRishtanataRecommendation(Guid id, [FromBody] RishtanataRecommendationSubmission submission, CancellationToken ct)
    {
        var result = await _workflowService.SubmitRishtanataRecommendationAsync(CurrentUserId, id, submission, ct);
        return result.IsAllowed ? Ok() : StatusCode(403, result.Message);
    }

    [HttpPut("amir-approval")]
    [Authorize(Policy = "CanFillAmirApprovalSection")]
    public async Task<IActionResult> SubmitAmirApproval(Guid id, [FromBody] AmirApprovalSubmission submission, CancellationToken ct)
    {
        var result = await _workflowService.ApproveByAmirAsync(CurrentUserId, id, submission, ct);
        return result.IsAllowed ? Ok() : StatusCode(403, result.Message);
    }
}