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

        try
        {
            // Authenticate using ChandaNo + PASSWORD
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

        // Get member using chandaNo
        var jamaatMember =
            await _gatewayHandler.GetMemberByChandaNoAsync(model.ChandaNo);

        if (jamaatMember is null)
        {
            ModelState.AddModelError(
                string.Empty,
                "We could not find your member account.");

            return View(model);
        }

        // Create/update local member
        await _jamaatMemberService.CreateOrUpdateAsync(jamaatMember);

        // Check Rishtanata Secretary
        var rishtanataSecretaryChandaNo =
            _configuration["RishtanataSecretary:ChandaNo"];

        var isRishtanataSecretary =
            !string.IsNullOrWhiteSpace(rishtanataSecretaryChandaNo) &&
            jamaatMember.ChandaNo == rishtanataSecretaryChandaNo;

        // Create authentication cookie
        await _cookieAuthService.SignInAsync(jamaatMember);

        // Return URL
        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
            Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        // Rishtanata Secretary
        if (isRishtanataSecretary)
        {
            return RedirectToAction(
                "Dashboard",
                "RishtanataSecretary");
        }

        // Other roles
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

    private IActionResult RedirectUserToDashboard(JamaatMember member)
    {
        // Role name comes from the gateway/DB and may be missing or
        // capitalized (e.g. "Jamaat Secretary"); normalize before matching.
        var roleName = (member.Role?.Name ?? string.Empty)
            .Trim()
            .ToLowerInvariant();

        return roleName switch
        {
            RoleNames.RishtanataSecretary =>
                RedirectToAction(
                    "Dashboard",
                    "RishtanataSecretary"),

            // The "Jama'at Secretary" dashboard is currently implemented by
            // JamaatPresidentController, which is gated by the
            // RequireJamaatSecretary policy (i.e. the "jamaat secretary"
            // role), so point jama'at secretaries at the action they are
            // actually allowed to open.
            RoleNames.JamaatSecretary =>
                RedirectToAction(
                    "Dashboard",
                    "JamaatPresident"),

            // No CircuitSecretaryController exists yet; fall back to the
            // member dashboard instead of a 404.
            RoleNames.CircuitSecretary =>
                RedirectToAction(
                    "Index",
                    "JamaatMemberDashboard"),

            // Ordinary members and any unrecognized/null role land here.
            _ =>
                RedirectToAction(
                    "Index",
                    "JamaatMemberDashboard")
        };
    }
}