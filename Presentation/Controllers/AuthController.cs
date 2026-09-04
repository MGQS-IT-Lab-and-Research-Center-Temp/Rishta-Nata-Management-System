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
    public AuthController(
        IGatewayHandler gatewayHandler,
        ICookieAuthenticationService cookieAuthService,
        IConfiguration configuration,
        IJamaatMemberService jamaatMemberService)
    {
        _gatewayHandler = gatewayHandler;
        _cookieAuthService = cookieAuthService;
        _configuration = configuration;
        _jamaatMemberService = jamaatMemberService;
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
        try
        {
            var tokenRequest = new TokenRequest(
                model.ChandaNo,
                model.Password);
            var tokenResponse =
                await _gatewayHandler.GenerateToken(tokenRequest);
            if (tokenResponse is null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid ChandaNo or Password.");
                return View(model);
            }
            if (!tokenResponse.Status ||
                string.IsNullOrWhiteSpace(tokenResponse.Token))
            {
                ModelState.AddModelError(
                    string.Empty,
                    string.IsNullOrWhiteSpace(tokenResponse.Message)
                        ? "Login failed."
                        : tokenResponse.Message);
                return View(model);
            }
        }
        catch (Exception)
        {
            ModelState.AddModelError(
                string.Empty,
                "Invalid Chanda number or password.");
            return View(model);
        }
        var jamaatMember =
            await _gatewayHandler.GetMemberByChandaNoAsync(model.ChandaNo);
        if (jamaatMember is null)
        {
            ModelState.AddModelError(
                string.Empty,
                "We could not find your member account.");
            return View(model);
        }
        var savedMember = await _jamaatMemberService.CreateOrUpdateAsync(jamaatMember);
        var rishtanataSecretaryChandaNo =
            _configuration["RishtanataSecretary:ChandaNo"];
        var isRishtanataSecretary =
            !string.IsNullOrWhiteSpace(rishtanataSecretaryChandaNo) &&
            savedMember.ChandaNo == rishtanataSecretaryChandaNo;
        await _cookieAuthService.SignInAsync(savedMember);
        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
            Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }
        if (isRishtanataSecretary)
        {
            return RedirectToAction(
                "Dashboard",
                "RishtanataSecretary");
        }
        return RedirectUserToDashboard(savedMember);
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
        var roleNames = member.MemberRoles
            .Select(mr => (mr.Role?.Name ?? string.Empty).Trim().ToLowerInvariant())
            .ToHashSet();

        if (roleNames.Contains(RoleNames.RishtanataSecretary))
        {
            return RedirectToAction("Dashboard", "RishtanataSecretary");
        }

        if (roleNames.Contains(RoleNames.JamaatSecretary))
        {
            return RedirectToAction("Dashboard", "JamaatPresident");
        }

        if (roleNames.Contains(RoleNames.CircuitSecretary))
        {
            return RedirectToAction("Index", "JamaatMemberDashboard");
        }

        return RedirectToAction("Index", "JamaatMemberDashboard");
    }
}