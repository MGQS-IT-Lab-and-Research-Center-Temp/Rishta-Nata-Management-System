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
    /// <summary>
    /// Handles GET requests for the login page
    /// </summary>
    /// <param name="returnUrl">Optional URL to redirect to after successful login</param>
    /// <returns>Returns the login view with the model containing return URL</returns>
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        // Create a new LoginViewModel instance with the provided return URL
        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        // Return the login view with the populated model
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
            // Authenticate using chandano and password
            var tokenRequest = new TokenRequest(
                model.ChandaNo,
                model.Password);
            // Authenticate using EMAIL + PASSWORD
            // var tokenRequest = new TokenRequest(model.Email,model.Password);

        var tokenResponse =
            await _gatewayHandler.GenerateToken(tokenRequest);

        if (tokenResponse is null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Invalid Chanda Number or password.");

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

        // Get member using EMAIL
        var jamaatMember =
            await _gatewayHandler.GetMemberByEmailAsync(model.Email);

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

        return RedirectToAction("Login","Auth");
    }

    private IActionResult RedirectUserToDashboard(
        JamaatMember member)
    {
        return member.Role.Name switch
        {
            RoleNames.JamaatSecretary =>
                RedirectToAction("Dashboard","JamaatSecretary"),

            RoleNames.CircuitSecretary =>
                RedirectToAction("Dashboard", "CircuitSecretary"),

            RoleNames.RishtanataSecretary =>
                RedirectToAction("Dashboard","RishtanataSecretary"),

            _ =>
                RedirectToAction("Dashboard","JamaatSecretary")
        };
    }
}