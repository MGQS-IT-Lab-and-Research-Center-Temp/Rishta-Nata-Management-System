using Application.Interfaces;
using Infrastructure.DTOs.Certificates;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class AqeeqahCertificateController : Controller
{
    private readonly IAqeeqahCertificateService _certificateService;

    public AqeeqahCertificateController(
        IAqeeqahCertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    // GET: /AqeeqahCertificate
    public async Task<IActionResult> Index()
    {
        var certificates =
            await _certificateService.GetAllCertificatesAsync();

        return View(certificates);
    }

    // GET: /AqeeqahCertificate/Create
    [HttpGet]
    public IActionResult Create()
    {
        var model = new AqeeqahCertificateDto
        {
            IssueDate = DateTime.Today
        };

        return View(model);
    }

    // POST: /AqeeqahCertificate/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AqeeqahCertificateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _certificateService.CreateCertificateAsync(model);

        return RedirectToAction(nameof(Index));
    }

    // GET: /AqeeqahCertificate/Certificate/{id}
    [HttpGet]
    public async Task<IActionResult> CertificateById(Guid id)
    {
        var certificate =
            await _certificateService.GetCertificateByIdAsync(id);

        if (certificate == null)
        {
            return NotFound();
        }

        return View(certificate);
    }

    // GET: /AqeeqahCertificate/Details/{id}
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var certificate =
            await _certificateService.GetCertificateByIdAsync(id);

        if (certificate == null)
        {
            return NotFound();
        }

        return View(certificate);
    }

    // GET: /AqeeqahCertificate/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _certificateService.DeleteCertificateAsync(id);

        return RedirectToAction(nameof(Index));
    }
}