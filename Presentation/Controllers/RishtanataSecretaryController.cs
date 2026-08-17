using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
namespace Presentation.Controllers
{
    using Application.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    namespace Presentation.Controllers
    {
        public class RishtanataSecretaryDashboardController : Controller
        {
            private readonly IRishtanataSecretaryService _service;

            public RishtanataSecretaryDashboardController(IRishtanataSecretaryService service)
            {
                _service = service;
            }

            
          

            // Dashboard page
            public async Task< IActionResult> Dashboard()
            {
                var dto = _service.GetDashboard();

                
                // var model = SecretaryMapper.ToViewModel(dto);
                // return View(model);

                return View(dto);
            }

            // Pending approvals page
            public  async Task<IActionResult> PendingApprovals()
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
}
