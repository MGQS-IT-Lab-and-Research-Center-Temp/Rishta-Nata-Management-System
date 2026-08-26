using Application.Interfaces;
using Application.Interfaces.Identity;
using Domain.Entities;
using Infrastructure.Identity.Tokens;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants.Roles;
using Presentation.Services.Auth;
using Presentation.ViewModels;


namespace Presentation.Controllers;

public class AuthController : Controller
{
    private readonly IGatewayHandler _gatewayHandler;
    private readonly ICookieAuthenticationService _cookieAuthService;
    private readonly IConfiguration _configuration;
    private readonly IJamaatMemberService _jamaatMemberService;

    public AuthController(IGatewayHandler gatewayHandler, ICookieAuthenticationService cookieAuthService, IConfiguration configuration, IJamaatMemberService jamaatMemberService)
    {
        _gatewayHandler = gatewayHandler;
        _cookieAuthService = cookieAuthService;
        _configuration = configuration;
        _jamaatMemberService = jamaatMemberService;
    }

    // GET: /Auth/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    // POST: /Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var tokenRequest = new TokenRequest(model.ChandaNo, model.Password);
        try
        {

            var tokenResponse = await _gatewayHandler.GenerateToken(tokenRequest);

            if (tokenResponse is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Chanda number or password.");

                return View(model);
            }

        var chandaNoInt = Convert.ToInt32(model.ChandaNo);
        var jamaatMember = await _gatewayHandler.GetMemberByChandaNoAsync(chandaNoInt);

        if (jamaatMember is null)
        {
            ModelState.AddModelError(string.Empty, "We could not find your member account.");

            return View(model);
        }

        await _jamaatMemberService.CreateOrUpdateAsync(jamaatMember);


        await _cookieAuthService.SignInAsync(jamaatMember);

        // Create authentication cookie
        await _cookieAuthService.SignInAsync(
            jamaatMember);

        // Return URL
        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
            Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectUserToDashboard(jamaatMember);
    }

    // POST: /Auth/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _cookieAuthService.SignOutAsync();
        return RedirectToAction("Login", "Auth");
    }

    private IActionResult RedirectUserToDashboard(JamaatMember memberViewModel)
    {
        return memberViewModel.Role.Name switch
        {
            RoleNames.JamaatSecretary =>
            RedirectToAction("Dashboard", "JamaatPresidentDashboard"),

            RoleNames.CircuitSecretary =>
                RedirectToAction("Dashboard", "CircuitSecretary"),

            RoleNames.RishtanataSecretary =>
            RedirectToAction("Dashboard", "RishtanataSecretary"),

            _ =>
                RedirectToAction("Dashboard", "MemberDashboard")
        };
    }
}