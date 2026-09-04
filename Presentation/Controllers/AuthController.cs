using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants.Roles;
using Presentation.Services.Auth;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class AuthController : Controller
{
    private readonly ICookieAuthenticationService _cookieAuthService;
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;

    public AuthController(
        ICookieAuthenticationService cookieAuthService,
        IConfiguration configuration,
        IAuthService authService)
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

        return RedirectUserToDashboard();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _cookieAuthService.SignOutAsync();

        return RedirectToAction("Login", "Auth");
    }

    private IActionResult RedirectUserToDashboard()
    {
        if (User.IsInRole(RoleNames.RishtanataSecretary))
        {
            return RedirectToAction("Dashboard", "RishtanataSecretary");
        }

        if (User.IsInRole(RoleNames.JamaatSecretary))
        {
            return RedirectToAction("Dashboard", "JamaatMemberDashboard");
        }

        if (User.IsInRole(RoleNames.CircuitSecretary))
        {
            return RedirectToAction(
                "Dashboard",
                "CircuitSecretary");
        }

        return RedirectToAction("Index", "JamaatMemberDashboard");
    }
}