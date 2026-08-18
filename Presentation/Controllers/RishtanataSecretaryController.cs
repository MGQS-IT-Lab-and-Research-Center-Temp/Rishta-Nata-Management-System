using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.Mapping.RishtanataSecretary;

namespace Presentation.Controllers
{
    public class RishtanataSecretaryController : Controller
    {
        private readonly IRishtanataSecretaryService _service;

        public RishtanataSecretaryController(IRishtanataSecretaryService service)
        {
            _service = service;
        }




        // Dashboard page
        public IActionResult Dashboard()
        {
            var dto = _service.GetDashboard();

            var model = RishtanataSecretaryDashboardMapping.ToViewModel(dto);

            return View(model);
        }

        // Pending approvals page
        public async Task<IActionResult> PendingApprovals()
        {
            var pendingApprovals = _service.GetPendingApprovals();

            return View(pendingApprovals);
        }
        // MarriedCouples Page
        public async Task<IActionResult> MarriedCouples()
        {
            var marriedCouples = _service.GetMarriedCouples();
            return View(marriedCouples);
        }

        // Review a specific application
        public async Task<IActionResult> Review(Guid id)
        {
            var application = _service.GetById(id);

            return View(application);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            _service.Approve(id);

            return RedirectToAction(nameof(PendingApprovals));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id)
        {
            _service.Reject(id);

            return RedirectToAction(nameof(PendingApprovals));
        }
    }
}