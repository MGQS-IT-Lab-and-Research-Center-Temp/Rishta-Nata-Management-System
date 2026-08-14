using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    public class AmirDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
