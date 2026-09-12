using System;
using System.Linq;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Services;

public class DashboardRedirector : IDashboardRedirector
{
    public IActionResult Resolve(JamaatMember member)
    {
        var roles = (member.Roles ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        bool HasRole(string role) =>
            roles.Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));

        if (HasRole(RoleNames.RishtanataSecretary))
            return RedirectTo("Dashboard", "RishtanataSecretary");

        if (HasRole(RoleNames.NaibRishtanataSecretary) || HasRole(RoleNames.GenSecRistanataDept))
            return RedirectTo("Dashboard", "AssistantRishtanataSecretary");

        if (HasRole(RoleNames.Amir))
            return RedirectTo("Dashboard", "Amir");

        if (HasRole(RoleNames.MissionaryInCharge))
            return RedirectTo("Dashboard", "MissionaryInCharge");

        if (roles.Any(r => r.Contains("imam", StringComparison.OrdinalIgnoreCase))
            || roles.Any(r => r.Contains("missionary", StringComparison.OrdinalIgnoreCase)))
            return RedirectTo("Dashboard", "Imam");

        if (HasRole(RoleNames.CircuitPresident))
            return RedirectTo("Dashboard", "CircuitPresident");

        if (HasRole(RoleNames.JamaatPresident))
            return RedirectTo("Dashboard", "JamaatPresident");

        return RedirectTo("Index", "JamaatMemberDashboard");
    }

    private static RedirectToActionResult RedirectTo(string action, string controller) =>
        new(action, controller, null);
}
