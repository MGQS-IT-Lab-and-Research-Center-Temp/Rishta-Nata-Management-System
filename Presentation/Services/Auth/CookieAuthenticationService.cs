using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Presentation.Constants.Roles;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
namespace Presentation.Services.Auth;

public class CookieAuthenticationService : ICookieAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task SignInAsync(JamaatMember jamaatMember, IEnumerable<string> roles)
    {
        var roleNames = roles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var secretaryChandaNo = _configuration["RishtanataSecretary:ChandaNo"];

        var isRishtanataSecretary =
            !string.IsNullOrWhiteSpace(secretaryChandaNo) && jamaatMember.ChandaNo == secretaryChandaNo;

        if (isRishtanataSecretary && !roleNames.Contains(RoleNames.RishtanataSecretary, StringComparer.OrdinalIgnoreCase))
        {
            roleNames.Add(RoleNames.RishtanataSecretary);
        }

        var claims = BuildClaims(jamaatMember, roles);

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    public async Task SignOutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    static List<Claim> BuildClaims(JamaatMember jamaatMember, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, jamaatMember.Id.ToString()),
            new(ClaimTypes.Name, jamaatMember.ChandaNo),
            new("jamaat", jamaatMember.JamaatName)
        };

        foreach (var role in roles .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if(roles.Contains(RoleNames.CircuitSecretary, StringComparer.OrdinalIgnoreCase))
        {
            claims.Add(
                new Claim(
                    "Circuit",
                    jamaatMember.CircuitName
                    ));
        }

        return claims;
    }
}