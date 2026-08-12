using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class JamaatPresidentController : Controller
{
    public IActionResult Dashboard()
    {
        var dashboard = new JamaatPresidentDashboardViewModel
        {
            PresidentName = "Abdul Rahman Muhammad",
            JamaatName = "Yaba Jamaat",
            CircuitName = "Lagos Mainland",

            PendingNikahReviews = 8,
            ReviewedToday = 5,
            TotalNikahApplications = 42,

            PendingApplications = new List<NikahApplicationViewModel>
        {
            new()
            {
                Id = 1,
                GroomName = "Muhammad Abdul Rahman",
                BrideName = "Aisha Yusuf",
                JamaatName = "Yaba Jamaat",
                SubmittedDate = DateTime.Today.AddDays(-1),
                Status = "Pending Review"
            },

            new()
            {
                Id = 2,
                GroomName = "Ibrahim Musa",
                BrideName = "Maryam Ibrahim",
                JamaatName = "Yaba Jamaat",
                SubmittedDate = DateTime.Today.AddDays(-2),
                Status = "Pending Review"
            },

            new()
            {
                Id = 3,
                GroomName = "Abdul Kareem",
                BrideName = "Fatimah Ali",
                JamaatName = "Yaba Jamaat",
                SubmittedDate = DateTime.Today.AddDays(-3),
                Status = "Pending Review"
            },

            new()
            {
                Id = 4,
                GroomName = "Yusuf Ahmed",
                BrideName = "Hauwa Bello",
                JamaatName = "Yaba Jamaat",
                SubmittedDate = DateTime.Today.AddDays(-4),
                Status = "Pending Review"
            }
        },

            RecentActivities = new List<RecentActivityViewModel>
        {
            new()
            {
                Description = "Nikah application submitted by Muhammad Abdul Rahman",
                Date = DateTime.Now.AddHours(-2)
            },

            new()
            {
                Description = "Nikah application reviewed",
                Date = DateTime.Now.AddHours(-5)
            },

            new()
            {
                Description = "Nikah application submitted by Ibrahim Musa",
                Date = DateTime.Now.AddDays(-1)
            }
        }
        };


        if (TempData["ApprovedApplicationId"] != null)
        {
            int approvedId = (int)TempData["ApprovedApplicationId"];

            var approvedApplication = dashboard.PendingApplications.FirstOrDefault(x => x.Id == approvedId);

            if (approvedApplication != null)
            {
                approvedApplication.Status = "Pending Admin Review";
            }
        }
        if (TempData["RejectedApplicationId"] != null)
        {
            int rejectedId = (int)TempData["RejectedApplicationId"];

            var rejectedApplication = dashboard.PendingApplications.FirstOrDefault(x => x.Id == rejectedId);

            if (rejectedApplication != null)
            {
                rejectedApplication.Status = "Rejected by Jamaat President";
            }
        }
        if (TempData["InformationRequiredApplicationId"] != null)
        {
            int informationId =
                (int)TempData["InformationRequiredApplicationId"];

            var application = dashboard.PendingApplications
                .FirstOrDefault(x => x.Id == informationId);

            if (application != null)
            {
                application.Status = "More Information Required";
            }
        }

        return View(dashboard);
    }
    public IActionResult Review(int id)
    {
        var application = new NikahApplicationViewModel
        {
            Id = id,
            GroomName = "Muhammad Abdul Rahman",
            BrideName = "Aisha Yusuf",
            JamaatName = "Yaba Jamaat",
            SubmittedDate = new DateTime(2026, 8, 11),
            Status = "Pending Review"
        };

        return View(application);
    }
    [HttpPost]
    public IActionResult Approve(int id)
    {
        TempData["ApprovedApplicationId"] = id;

        TempData["Success"] =
            $"Nikah application #{id} has been approved by the Jamaat President and forwarded to Admin Review.";

        return RedirectToAction("Dashboard");
    }
    [HttpPost]
    public IActionResult Reject(int id)
    {
        TempData["RejectedApplicationId"] = id;

        TempData["Success"] =
            $"Nikah application #{id} has been rejected by the Jamaat President.";

        return RedirectToAction("Dashboard");
    }
    [HttpPost]
    public IActionResult RequestMoreInformation(int id)
    {
        TempData["InformationRequiredApplicationId"] = id;

        TempData["Success"] =
            $"More information has been requested for Nikah application #{id}.";

        return RedirectToAction("Dashboard");
    }
}