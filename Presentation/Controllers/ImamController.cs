using Application.Interfaces;
using Application.Workflow;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Imam;
using System.Security.Claims;

namespace Presentation.Controllers;

[Authorize(Policy = "CanFillImamSignoffSection")]
public class ImamController : Controller
{
    private readonly IImamSignoffService _signoffService;
    private readonly IMarriageApplicationFormService _formService;
    private readonly IMarriageFormWorkflowService _workflowService;
    private readonly IStageAuthorizationService _authorizationService;

    public ImamController(
        IImamSignoffService signoffService,
        IMarriageApplicationFormService formService,
        IMarriageFormWorkflowService workflowService,
        IStageAuthorizationService authorizationService)
    {
        _signoffService = signoffService;
        _formService = formService;
        _workflowService = workflowService;
        _authorizationService = authorizationService;
    }

    private string CurrentMembershipNo =>
        User.FindFirstValue(ClaimNames.MembershipNo)
        ?? User.FindFirstValue(ClaimTypes.Name)
        ?? string.Empty;

    public async Task<IActionResult> Dashboard(CancellationToken ct)
    {
        var pending = await _signoffService.GetPendingAsync(CurrentMembershipNo, ct);

        var model = new ImamDashboardViewModel
        {
            PendingSignoffs = pending
                .Select(dto => new ImamDashboardViewModel.ImamPendingItem
                {
                    FormId = dto.FormId,
                    ReferenceNumber = dto.ReferenceNumber,
                    BrideName = dto.BrideName,
                    BridegroomName = dto.BridegroomName,
                    ApprovedDateOfNikah = dto.ApprovedDateOfNikah,
                    Venue = dto.Venue,
                    SignatureDate = dto.SignatureDate
                })
                .ToList()
        };

        return View(model);
    }

    [HttpGet("Imam/Signoff/{id:guid}")]
    public async Task<IActionResult> Signoff(Guid id, CancellationToken ct)
    {
        var authorization = await _authorizationService.CanUserActAsync(
            CurrentMembershipNo, id, MarriageFormStage.AwaitingImamSignoff, ct);
        if (!authorization.IsAllowed)
        {
            return NotFound("Application not found.");
        }

        var form = await _formService.GetByIdAsync(id, ct);
        if (form is null)
        {
            return NotFound("Application not found.");
        }

        var model = new ImamSignoffViewModel
        {
            FormId = form.Id,
            ReferenceNumber = form.ReferenceNumber,
            BrideName = form.BrideName,
            BridegroomName = form.BridegroomName,
            ApprovedDateOfNikah = form.ApprovedDateOfNikah,
            Venue = form.Venue,
            Name = form.OfficiatingImamName,
            AddressJamaat = form.OfficiatingImamAddressJamaat,
            Tel = form.ImamVerification?.Tel ?? string.Empty,
            SignatureDate = form.OfficiatingImamSignatureDate
        };

        return View(model);
    }

    [HttpPost("Imam/Signoff/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Signoff(Guid id, ImamSignoffViewModel model, CancellationToken ct)
    {
        var submission = new ImamSignoffSubmission(
            model.Name ?? string.Empty,
            model.AddressJamaat ?? string.Empty,
            model.Tel ?? string.Empty,
            model.SignatureDate ?? string.Empty);

        var result = await _workflowService.SubmitImamSignoffAsync(CurrentMembershipNo, id, submission, ct);

        if (!result.IsAllowed)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["Success"] = "Sign-off recorded. The application is complete.";
        return RedirectToAction(nameof(Dashboard));
    }
}