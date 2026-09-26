using Application.DivorceEvidence;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents;

/// <summary>
/// Download links for a form's divorce certificates (Gap 8), for the review
/// pages. Renders nothing when none are on file. The Download action
/// re-checks access on every request.
/// </summary>
/// <remarks>
/// This component does no authorization of its own: it lists the original file
/// names for any form id. Only invoke it from an action that has already
/// authorized the caller to view the form's documents (see
/// IStageAuthorizationService.CanViewFormDocumentsAsync).
/// </remarks>
public class DivorceEvidenceLinksViewComponent : ViewComponent
{
    private readonly IDivorceEvidenceService _divorceEvidence;

    public DivorceEvidenceLinksViewComponent(IDivorceEvidenceService divorceEvidence)
    {
        _divorceEvidence = divorceEvidence;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid formId)
    {
        var documents = await _divorceEvidence.ListAsync(formId, HttpContext.RequestAborted);

        return View(new DivorceEvidenceLinksModel(formId, documents));
    }
}

public sealed record DivorceEvidenceLinksModel(
    Guid FormId,
    IReadOnlyList<DivorceEvidenceDocumentInfo> Documents);
