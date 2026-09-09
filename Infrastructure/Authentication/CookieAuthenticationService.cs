using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
namespace Presentation.Services.Auth;

public class CookieAuthenticationService : ICookieAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignInAsync(JamaatMember jamaatMember, IEnumerable<string> roles)
    {
        var identity = new ClaimsIdentity(
            BuildClaims(jamaatMember, roles),
            CookieAuthenticationDefaults.AuthenticationScheme);

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
            new(ClaimNames.MembershipNo, jamaatMember.ChandaNo),
            new(ClaimNames.FullName, BuildFullName(jamaatMember))
        };

        foreach (var role in roles .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim(ClaimNames.MemberRole, role));
        }

        return claims;
    }

    static string BuildFullName(JamaatMember member)
    {
        var parts = new[] { member.FirstName, member.MiddleName, member.Surname }
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Select(part => part!.Trim());

        return string.Join(" ", parts);
    }
}
