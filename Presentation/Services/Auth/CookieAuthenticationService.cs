using Domain.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Presentation.Constants.Roles;
namespace Presentation.Services.Auth;


public class CookieAuthenticationService : ICookieAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignInAsync(JamaatMember jamaatMember, bool isRishtanataSecretary)
    {
        var role = isRishtanataSecretary ? RoleNames.RishtanataSecretary : jamaatMember.Role.Name;
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, jamaatMember.Id.ToString()),
                new Claim(ClaimTypes.Name, jamaatMember.ChandaNo),
                new Claim(ClaimTypes.Role, role.ToLowerInvariant()),
                new Claim("HierarchyLevel", jamaatMember.Role.HierarchyLevel.ToString()),
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

        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext!.SignInAsync("MyCookieAuth", principal);
    }

    public async Task SignOutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync();
    }

}
