using System.Security.Claims;
using Application.Authorization;
using Application.Interfaces;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

/// <summary>
/// Streams an uploaded divorce certificate as an attachment (Gap 8). Who may
/// download is decided by IStageAuthorizationService.CanViewFormDocumentsAsync.
/// </summary>
[Authorize]
public class DivorceEvidenceController : Controller
{
    private readonly IStageAuthorizationService _stageAuthorization;
    private readonly IDivorceEvidenceService _divorceEvidence;

    public DivorceEvidenceController(
        IStageAuthorizationService stageAuthorization,
        IDivorceEvidenceService divorceEvidence)
    {
        _stageAuthorization = stageAuthorization;
        _divorceEvidence = divorceEvidence;
    }

    // GET: /DivorceEvidence/Download/{id}?party=Bride
    [HttpGet]
    public async Task<IActionResult> Download(Guid id, DivorceEvidenceParty party, CancellationToken ct)
    {
        if (!Enum.IsDefined(party))
        {
            return NotFound();
        }

        var access = await _stageAuthorization.CanViewFormDocumentsAsync(
            GetCurrentMembershipNo() ?? string.Empty, id, ct);

        if (!access.IsAllowed)
        {
            return access.Reason == StageAuthorizationDenyReason.FormNotFound
                ? NotFound()
                : Forbid();
        }

        var file = await _divorceEvidence.OpenAsync(id, party, ct);
        if (file is null)
        {
            return NotFound();
        }

        // A download name makes this Content-Disposition: attachment; nosniff
        // stops the browser second-guessing the stored content type.
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return File(file.Content, file.ContentType, file.FileName);
    }

    private string? GetCurrentMembershipNo() =>
        User.FindFirstValue(ClaimNames.MembershipNo)
        ?? User.FindFirstValue(ClaimTypes.Name);
}
