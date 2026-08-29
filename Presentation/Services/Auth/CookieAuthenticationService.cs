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

        var isRishtanataSecretary =
            !string.IsNullOrWhiteSpace(secretaryChandaNo) &&
            jamaatMember.ChandaNo == secretaryChandaNo;

        var roleNames = jamaatMember.MemberRoles
            .Select(mr => mr.Role.Name)
            .ToList();

        if (isRishtanataSecretary && !roleNames.Contains(RoleNames.RishtanataSecretary))
        {
            roleNames.Add(RoleNames.RishtanataSecretary);
        }
        var identity = new ClaimsIdentity(BuildClaims(jamaatMember, roleNames), CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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
            new(ClaimTypes.Name, jamaatMember.ChandaNo)
        };
        foreach (var roleName in roleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
        }
        var highestHierarchyLevel = jamaatMember.MemberRoles
            .Select(mr => mr.Role.HierarchyLevel)
            .DefaultIfEmpty(0)
            .Max();
        claims.Add(new Claim("HierarchyLevel", highestHierarchyLevel.ToString()));
        if (roleNames.Contains(RoleNames.JamaatSecretary))
        {
            claims.Add(new Claim("Jamaat", jamaatMember.JamaatName));
        }
        if (roleNames.Contains(RoleNames.CircuitSecretary))
        {
            claims.Add(new Claim("Circuit", jamaatMember.CircuitName));
        }
        return claims;
    }
}