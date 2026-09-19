// do page for review for individual nikkah form - azeez
// do page for viewing all certificates under the jama'at president's jama'at (for now view all certificates) - faridah
// fix all errors under your dto - faridah -done
// fix all errors under service and interface - yusroh
// ensure that dto namespace is infrastructure not application - done
// use the respective service to do all db operation in this controller

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Workflow;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Presentation.Mapping;

namespace Presentation.Controllers;

[Authorize(Policy = "RequireJamaatSecretary")]
public class JamaatPresidentController : Controller
{
    private readonly IJamaatPresidentService _service;
    private readonly ICertificateService _certificateService;
    private readonly IStageAuthorizationService _authorizationService;
    private readonly IMarriageFormWorkflowService _workflowService;

    public JamaatPresidentController(
        IJamaatPresidentService service,
        ICertificateService certificateService,
        IStageAuthorizationService authorizationService,
        IMarriageFormWorkflowService workflowService)
    {
        _service = service;
        _certificateService = certificateService;
        _authorizationService = authorizationService;
        _workflowService = workflowService;
    }

    // ============================================================
    // DASHBOARD
    // ============================================================

    public async Task<IActionResult> Dashboard()
    {
        var dto = await _service.GetDashboardAsync(
            GetCurrentUserId());

        return View(JamaatPresidentMapping.ToViewModel(dto));
    }

    // ============================================================
    // PENDING APPLICATIONS
    // ============================================================

    public async Task<IActionResult> PendingApplications()
    {
        var applications = await _service.GetPendingApplicationsAsync(
            GetCurrentUserId());

        return View(applications
            .Select(JamaatPresidentMapping.ToViewModel)
            .ToList());
    }

    // ============================================================
    // REVIEWED APPLICATIONS
    // ============================================================

    public async Task<IActionResult> ReviewedApplications()
    {
        var applications = await _service.GetReviewedApplicationsAsync(
            GetCurrentUserId());

        return View(applications
            .Select(JamaatPresidentMapping.ToViewModel)
            .ToList());
    }

    // ============================================================
    // REVIEW APPLICATION
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> Review(Guid id)
    {
        if (!await CanReviewAsync(id))
        {
            return NotFound("Marriage application or its form was not found.");
        }

        var dto = await _service.GetReviewByIdAsync(id);

        if (dto == null)
        {
            return NotFound("Marriage application or its form was not found.");
        }

        return View(JamaatPresidentMapping.ToViewModel(dto));
    }

    // ============================================================
    // APPROVE
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct)
    {
        if (!await CanReviewAsync(id))
        {
            return NotFound("Marriage application or its form was not found.");
        }

        var dto = await _service.GetReviewByIdAsync(id);

        if (dto == null)
        {
            return NotFound("Marriage application or its form was not found.");
        }

        var result = dto.CurrentFormStage switch
        {
            MarriageFormStage.AwaitingBrideJamaatPresident =>
                await _workflowService.SubmitJamaatPresidentVerificationAsync(
                    CurrentMembershipNo, id,
                    new JamaatPresidentVerificationSubmission(
                        dto.JamaatPresidentName,
                        dto.JamaatPresidentTel,
                        dto.JamaatPresidentSignatureDate),
                    ct),
            MarriageFormStage.AwaitingGroomJamaatPresident =>
                await _workflowService.SubmitGroomJamaatPresidentVerificationAsync(
                    CurrentMembershipNo, id,
                    new JamaatPresidentVerificationSubmission(
                        dto.GroomJamaatPresidentName,
                        dto.GroomJamaatPresidentTel,
                        dto.GroomJamaatPresidentSignatureDate),
                    ct),
            _ => null
        };

        if (result == null)
        {
            TempData["Error"] =
                "This application is no longer awaiting Jama'at President review.";
        }
        else if (!result.IsAllowed)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] =
                "Nikah application approved and forwarded to the National Rishtanata Secretary.";
        }

        return RedirectToAction(nameof(Dashboard));
    }

    // ============================================================
    // MARRIAGE CERTIFICATES
    // ============================================================

    /// <summary>
    /// Displays all marriage certificates.
    ///
    /// For now, all certificates are displayed.
    /// Later, this can be filtered by the Jama'at President's Jama'at.
    /// </summary>
    public async Task<IActionResult> Certificates()
    {
        var certificates = await _certificateService.GetAllCertificatesAsync();

        var viewModels = certificates
            .Select(JamaatPresidentMapping.ToViewModel)
            .ToList();

        return View(viewModels);
    }

    // ============================================================
    // CURRENT USER
    // ============================================================

    private string CurrentMembershipNo =>
        User.FindFirstValue(ClaimNames.MembershipNo)
        ?? User.FindFirstValue(ClaimTypes.Name)
        ?? string.Empty;

    private async Task<bool> CanReviewAsync(Guid id)
    {
        var brideStage = await _authorizationService.CanUserActAsync(
            CurrentMembershipNo, id, MarriageFormStage.AwaitingBrideJamaatPresident);

        if (brideStage.IsAllowed)
        {
            return true;
        }

        var groomStage = await _authorizationService.CanUserActAsync(
            CurrentMembershipNo, id, MarriageFormStage.AwaitingGroomJamaatPresident);

        return groomStage.IsAllowed;
    }

    private Guid? GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userId, out var id))
        {
            return id;
        }

        return null;
    }
}