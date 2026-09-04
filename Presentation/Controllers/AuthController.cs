using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class AuthController : Controller
{
    private readonly ICookieAuthenticationService _cookieAuthService;
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;

    public AuthController(ICookieAuthenticationService cookieAuthService, IConfiguration configuration, IAuthService authService)
    {
        _cookieAuthService = cookieAuthService;
        _configuration = configuration;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.LoginAsync(model.ChandaNo, model.Password);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Login failed.");

            return View(model);
        }

        var member = result.Member!;

        await _cookieAuthService.SignInAsync(member, result.Roles);

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectUserToDashboard(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _cookieAuthService.SignOutAsync();

        return RedirectToAction("Login", "Auth");
    }

    private IActionResult RedirectUserToDashboard(JamaatMember member)
    {
        var rishtanataSecretaryChandaNo = _configuration["RishtanataSecretary:ChandaNo"];

        if (!string.IsNullOrWhiteSpace(rishtanataSecretaryChandaNo) &&
            string.Equals(member.ChandaNo, rishtanataSecretaryChandaNo, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction("Dashboard", "RishtanataSecretary");
        }

        if (User.IsInRole(RoleNames.NaibRishtanataSecretary) || User.IsInRole(RoleNames.GenSecRistanataDept))
        {
            return RedirectToAction("Dashboard", "AssistantRishtanataSecretary");
        }

        if (User.IsInRole(RoleNames.Amir))
        {
            return RedirectToAction("Dashboard", "Amir");
        }

        if (User.IsInRole(RoleNames.MissionaryInCharge))
        {
            return RedirectToAction("Dashboard", "MissionaryInCharge");
        }

        if (User.IsInRole(RoleNames.CircuitPresident))
        {
            return RedirectToAction("Dashboard", "CircuitPresident");
        }

        if (User.IsInRole(RoleNames.JamaatPresident))
        {
            return RedirectToAction("Dashboard", "JamaatPresident");
        }

        if (User.IsInRole(RoleNames.Member))
        {
            return RedirectToAction("Dashboard", "JamaatMemberDashboard");
        }

        return RedirectToAction("Index", "JamaatMemberDashboard");
    }
}