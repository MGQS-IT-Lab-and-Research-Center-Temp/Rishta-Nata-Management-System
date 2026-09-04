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
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Authentication;

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

        var isRishtanataSecretary =
            !string.IsNullOrWhiteSpace(secretaryChandaNo) && jamaatMember.ChandaNo == secretaryChandaNo;
        var role = isRishtanataSecretary ? RoleNames.RishtanataSecretary : jamaatMember.NewRole;

        var roleNames = jamaatMember.MemberRoles
            .Select(mr => mr.Role.Name)
            .ToList();

        if (isRishtanataSecretary && !roleNames.Contains(RoleNames.RishtanataSecretary))
        {
            roleNames.Add(RoleNames.RishtanataSecretary);
        }

        var identity = new ClaimsIdentity(BuildClaims(jamaatMember, roleNames),
            CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            principal);
        return isRishtanataSecretary
            ? RoleNames.RishtanataSecretary
            : roleNames.FirstOrDefault() ?? string.Empty;
    }

    public async Task SignOutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    static List<Claim> BuildClaims(JamaatMember jamaatMember, List<string> roleNames)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, jamaatMember.Id.ToString()),
            new(ClaimTypes.Name, jamaatMember.ChandaNo),
            new(ClaimTypes.Role, roleNames.First()),
        };

        claims.Add(new Claim("Jamaat", jamaatMember.JamaatName));

        if (roleNames.Contains(RoleNames.CircuitSecretary))
        {
            claims.Add(new Claim("Circuit", jamaatMember.CircuitName));
        }

        return claims;
    }
}
