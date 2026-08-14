using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class RishtanataSecretaryController : Controller
    {
        // ==========================
        // Dashboard
        // ==========================
        public IActionResult Dashboard()
        {
            var model = new RishtanataSecretaryDashboardViewModel
            {
                SecretaryName = "Alhaja Amina Bello",

                PendingApprovals = 4,
                ApprovedApplications = 27,
                RejectedApplications = 3,
                MarriedCouples = 65,
                TotalMembers = 420,

                PendingApplications = GetPendingApprovals(),

                RecentActivities = new List<RecentActivityViewModel>
                {
                    new RecentActivityViewModel
                    {
                        ApplicationNumber = "AMJN-2026-001",
                        Description = "Approved Nikah Application",
                        Date = DateTime.Now.AddHours(-2)
                    },

                    new RecentActivityViewModel
                    {
                        ApplicationNumber = "AMJN-2026-002",
                        Description = "Rejected Nikah Application",
                        Date = DateTime.Now.AddHours(-5)
                    },

                    new RecentActivityViewModel
                    {
                        ApplicationNumber = "AMJN-2026-003",
                        Description = "Returned application to Jama'at President",
                        Date = DateTime.Now.AddDays(-1)
                    }
                }
            };

            return View(model);
        }

        // ==========================
        // Pending Approvals
        // ==========================
        public IActionResult PendingApprovals()
        {
            var applications = GetPendingApprovals();

            return View(applications);
        }

        // ==========================
        // Review
        // ==========================
        public IActionResult Review(int id)
        {
            var model = new ReviewApplicationViewModel
            {
                Id = id,
                ApplicationNumber = $"NK-2026-{id:000}",

                GroomName = "Ahmad Ibrahim",
                BrideName = "Aisha Musa",

                GroomPhone = "08031234567",
                BridePhone = "08039876543",

                GroomAddress = "Lagos State",
                BrideAddress = "Lagos State",

                JamaatName = "Agege Jamaat",

                PresidentName = "Alh. Suleiman Musa",

                PresidentRecommendation = "Recommended for Approval",

                //PresidentRemarks =
                //    "All required documents have been verified. Applicant satisfies the Jama'at requirements.",

                SubmittedDate = DateTime.Now.AddDays(-3),

                Status = "Pending Secretary Review",

                IsApprovedByPresident = true
            };

            return View(model);
        }

        // ==========================
        // Married Couples
        // ==========================
        public IActionResult MarriedCouples()
        {
            var couples = new List<MarriedCoupleViewModel>
            {
                new MarriedCoupleViewModel
                {
                    Id = 1,
                    CertificateNumber = "MC-001",
                    HusbandName = "Ahmad Ibrahim",
                    WifeName = "Aisha Musa",
                    JamaatName = "Alakuko Jamaat",
                    MarriageDate = new DateTime(2026,1,12),
                    Status = "Registered"
                }
            };

            return View(couples);
        }

        // ==========================
        // Jama'at Members
        // ==========================
        public IActionResult JamaatMembers()
        {
            var members = new List<JamaatMemberViewModel>
            {
                new JamaatMemberViewModel
                {
                    Id = 1,
                    MemberNumber = "JM0001",
                    FullName = "Ahmad Ibrahim",
                    Gender = "Male",
                    JamaatName = "Agege Jamaat",
                    MaritalStatus = "Married",
                    Occupation = "Engineer",
                    PhoneNumber = "08030001111"
                }
            };

            return View(members);
        }

        // ==========================
        // Approve
        // ==========================
        [HttpPost]
        public IActionResult Approve(int id)
        {
            TempData["Success"] =
                $"Application NK-2026-{id:000} approved successfully.";

            return RedirectToAction(nameof(Dashboard));
        }

        // ==========================
        // Reject
        // ==========================
        [HttpPost]
        public IActionResult Reject(int id)
        {
            TempData["Error"] =
                $"Application NK-2026-{id:000} rejected.";

            return RedirectToAction(nameof(Dashboard));
        }

        // ==========================
        // Return to President
        // ==========================
        [HttpPost]
        public IActionResult ReturnToPresident(int id)
        {
            TempData["Warning"] =
                $"Application NK-2026-{id:000} returned to the Jama'at President.";

            return RedirectToAction(nameof(Dashboard));
        }

        // ==========================
        // Dummy Data
        // ==========================
        private List<PendingApprovalViewModel> GetPendingApprovals()
        {
            return new List<PendingApprovalViewModel>
            {
                new PendingApprovalViewModel
                {
                    Id = 1,
                    ApplicationNumber = "AMJN-2026-001",
                    GroomName = "Ahmad Ibrahim",
                    BrideName = "Aisha Musa",
                    JamaatName = "Agege Jamaat",
                    PresidentName = "Alh. Suleiman Musa",
                    SubmittedDate = DateTime.Now.AddDays(-3),
                    PresidentRecommendation = "Recommended",
                    Status = "Pending Secretary Review"
                },

                new PendingApprovalViewModel
                {
                    Id = 2,
                    ApplicationNumber = "AMJN-2026-004",
                    GroomName = "Abdul Kareem",
                    BrideName = "Zainab Hassan",
                    JamaatName = "Alakuko Jamaat",
                    PresidentName = "Alh. Sadiq Ibrahim",
                    SubmittedDate = DateTime.Today,
                    PresidentRecommendation = "Recommended",
                    Status = "Pending Secretary Review"
                }
            };
        }
    }
}
