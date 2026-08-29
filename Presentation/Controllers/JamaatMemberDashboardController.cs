using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;

namespace Presentation.Controllers;

public class JamaatMemberDashboardController : Controller
{
    public IActionResult Index()
    {
        var model = new MemberDashboardViewModel
        {
            MemberName = "Azeem",
            CurrentSpouse = new SpouseInfo
            {
                Name = "Hajarah",
                MarriageDate = new DateTime(2025, 3, 12)
            },
            MarriageHistory = new List<MarriageHistoryEntry>
            {
                new MarriageHistoryEntry { SpouseName = "Hiqmah", Status = "Married", Date = new DateTime(2018, 6, 20) },
                new MarriageHistoryEntry { SpouseName = "Hiqmah", Status = "Divorced", Date = new DateTime(2021, 11, 3) },
                new MarriageHistoryEntry { SpouseName = "Hajarah", Status = "Married", Date = new DateTime(2025, 3, 12) }
            }
        };

        return View(model);
    }
}