using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.Mapping.RishtanataSecretary;
using Application.Interfaces;

namespace Presentation.Controllers
{
    public class RishtanataSecretaryController : Controller
    {
        private readonly IRishtanataSecretaryService _service;
        private readonly IRoleAssignmentService _roleService;

        public RishtanataSecretaryController(IRishtanataSecretaryService service, IRoleAssignmentService roleService)
        {
            _service = service;
            _roleService = roleService;

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

            var model = marriedCouples.Select(MarriedCoupleMapping.ToViewModel).ToList();
            return View(model);
        }

        // Review a specific application
        public async Task<IActionResult> Reviews(Guid id)
        {
            var application = _service.GetById(id);

            return View(application);
        }
        // Gert All Jamaat Members
        public async Task<IActionResult> Members()
        {
            var members = _service.GetMembers();
            return View(members);
        }
        // Edit Role of a specific Jamaat Member
        public async Task<IActionResult> EditRoles(Guid id)
        {
            var dto = await _roleService.GetRoleManagementAsync(id);
            var viewModel = RoleManagementMapper.toViewModel(dto);
            return View(viewModel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(Guid memberId, Guid roleId)
        {
            var currentSecretary = User.Identity?.Name ?? "System"; 
            var (success, error) = await _roleService.AssignRoleAsync(memberId, roleId, currentSecretary);

            TempData[success ? "Success" : "Error"] = success ? "Role updated successfully." : error;
            return RedirectToAction(nameof(EditRoles), new { id = memberId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid memberId)
        {
            var currentSecretary = User.Identity?.Name ?? "System";
            var (success, error) = await _roleService.ResetToBaseRoleAsync(memberId, currentSecretary);

            TempData[success ? "Success" : "Error"] = success ? "Role reset to Jama'at Member." : error;
            return RedirectToAction(nameof(EditRoles), new { id = memberId });
        }
    }
}