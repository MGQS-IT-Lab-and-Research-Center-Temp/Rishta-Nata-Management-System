using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class MarriageApplicationDetailsController : Controller
{
    private readonly IBridegroomService _bridegroomService;
    private readonly IRepresentativeService _representativeService;

    public MarriageApplicationDetailsController(
        IBridegroomService bridegroomService,
        IRepresentativeService representativeService)
    {
        _bridegroomService = bridegroomService;
        _representativeService = representativeService;
    }

    public async Task<IActionResult> Bridegroom(Guid id)
    {
        var bridegroom =
            await _bridegroomService.GetByApplicationIdAsync(id);

        if (bridegroom == null)
        {
            return NotFound();
        }

        return View(bridegroom);
    }

    public async Task<IActionResult> Representative(Guid id)
    {
        var representative =
            await _representativeService.GetByApplicationIdAsync(id);

        if (representative == null)
        {
            return NotFound();
        }

        return View(representative);
    }
}