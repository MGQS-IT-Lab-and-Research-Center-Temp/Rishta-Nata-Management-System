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

        public IActionResult Dashboard()
        {
            var dto = _service.GetDashboard();

            //var model = SecretaryMapper.ToViewModel(dto);

            return View(dto);
        }

        public IActionResult PendingApprovals()
        {
            var dto = _service.GetPendingApprovals();

            var model = dto
                //.Select(SecretaryMapper.ToViewModel)
                .ToList();

            return View(model);
        }

        public IActionResult Review(Guid id)
        {
            var dto = _service.GetById(id);

            //var model = SecretaryMapper.ToViewModel(dto);

            return View(dto);
        }

        [HttpPost]
        public IActionResult Approve(Guid id)
        {
            _service.Approve(id);

            return RedirectToAction(nameof(PendingApprovals));
        }

        [HttpPost]
        public IActionResult Reject(Guid id)
        {
            _service.Reject(id);

            return RedirectToAction(nameof(PendingApprovals));
        }
    }
}