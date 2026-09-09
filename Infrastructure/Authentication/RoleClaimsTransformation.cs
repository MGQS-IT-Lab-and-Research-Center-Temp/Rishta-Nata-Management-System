using System.Security.Claims;
using Domain.Constants;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication;

/// <summary>
/// Mid-session role refresh. Roles are issued at login from the Tajneed login
/// response and persisted on JamaatMember.Roles; this transformation re-syncs the
/// ClaimTypes.Role / member_roles claims from that value on each authenticated
/// request, so a member whose roles were updated on the local member row picks up
/// the change without logging in again (docs/bugs-and-gaps.md #3).
///
/// This only mirrors existing claims — it never grants roles that are not present
/// on the member record, and it short-circuits when the claims already match.
/// </summary>
public class RoleClaimsTransformation : IClaimsTransformation
{
    private readonly RishtanataDbContext _context;

    public RoleClaimsTransformation(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return principal;
        }

        var membershipNo = principal.FindFirstValue(ClaimNames.MembershipNo);
        if (string.IsNullOrWhiteSpace(membershipNo))
        {
            return principal;
        }

        var roles = await _context.JamaatMembers
            .AsNoTracking()
            .Where(m => m.ChandaNo == membershipNo)
            .Select(m => m.Roles)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(roles))
        {
            return principal;
        }

        var identity = principal.Identity as ClaimsIdentity;
        if (identity is null)
        {
            return principal;
        }

        var fresh = roles
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();

        if (fresh.Length == 0)
        {
            return principal;
        }

        var current = identity.FindAll(c => c.Type is ClaimTypes.Role or ClaimNames.MemberRole)
            .Select(c => c.Value.Trim())
            .Where(value => value.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (fresh.All(f => current.Contains(f)) && current.All(c => fresh.Contains(c)))
        {
            return principal;
        }

        foreach (var claim in identity.FindAll(c => c.Type is ClaimTypes.Role or ClaimNames.MemberRole).ToList())
        {
            identity.RemoveClaim(claim);
        }

        foreach (var role in fresh)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim(ClaimNames.MemberRole, role));
        }

        return principal;
    }
}