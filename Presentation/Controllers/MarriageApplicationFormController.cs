using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class MarriageApplicationFormController : Controller
    {
        private readonly RishtanataDbContext _context;

        public MarriageApplicationFormController(RishtanataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarriageApplicationForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Set<MarriageApplicationForm>().Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}