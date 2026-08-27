using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Presentation.Constants.Roles;
using System.Security.Claims;

namespace Presentation.Services.Auth;


public class CookieAuthenticationService : ICookieAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public CookieAuthenticationService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<string> SignInAsync(JamaatMember jamaatMember)
    {
        var secretaryChandaNo = _configuration["RishtanataSecretary:ChandaNo"];

        var role = !string.IsNullOrWhiteSpace(secretaryChandaNo) && jamaatMember.ChandaNo == secretaryChandaNo ? RoleNames.RishtanataSecretary : jamaatMember.Role.Name;

        var identity = new ClaimsIdentity(BuildClaims(jamaatMember, role), CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return role;
    }
    public async Task SignOutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    static  List<Claim> BuildClaims(JamaatMember jamaatMember, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, jamaatMember.Id.ToString()),
            new(ClaimTypes.Name, jamaatMember.ChandaNo),
            new(ClaimTypes.Role, role),
            new("HierarchyLevel", jamaatMember.Role.HierarchyLevel.ToString())
        };

        switch (jamaatMember.Role.Name)
        {
            case RoleNames.JamaatSecretary:
                claims.Add(new Claim("Jamaat", jamaatMember.JamaatName));
                break;
            case RoleNames.CircuitSecretary:
                claims.Add(new Claim("Circuit", jamaatMember.CircuitName));
                break;
        }

        return claims;
    }


}