using Domain.Entities;

namespace Application.Interfaces;

public interface IJamaatMemberService
{
    Task<JamaatMember> CreateOrUpdateAsync(JamaatMember jamaatMember);

    /// <summary>
    /// Loads the local member row by ChandaNo, or returns null when absent.
    /// </summary>
    Task<JamaatMember?> GetByChandaNoAsync(string chandaNo, CancellationToken cancellationToken = default);

    /// <summary>
    /// True when the local profile is recent enough that we can skip a fresh
    /// fetch from the Tajneed member endpoint. Defaults to 24 hours.
    /// </summary>
    bool IsProfileFresh(JamaatMember member, TimeSpan? maxAge = null);

    /// <summary>
    /// Updates only the Roles field on an existing local member row (roles
    /// come from the Tajneed token response, not the member profile fetch).
    /// </summary>
    Task<JamaatMember> UpdateRolesAsync(string chandaNo, IEnumerable<string> roles, CancellationToken cancellationToken = default);
}
