using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Services;
using Presentation.ViewModels;

namespace Presentation.Controllers;

public class AuthController : Controller
{
    private readonly ICookieAuthenticationService _cookieAuthService;
    private readonly IAuthService _authService;
    private readonly IDashboardRedirector _dashboardRedirector;

    public AuthController(
        ICookieAuthenticationService cookieAuthService,
        IAuthService authService,
        IDashboardRedirector dashboardRedirector)
    {
        _cookieAuthService = cookieAuthService;
        _authService = authService;
        _dashboardRedirector = dashboardRedirector;
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

        return _dashboardRedirector.Resolve(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _cookieAuthService.SignOutAsync();

        return RedirectToAction("Login", "Auth");
    }
}
