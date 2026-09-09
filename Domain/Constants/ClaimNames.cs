namespace Domain.Constants;

/// <summary>
/// Custom claim type names used for stage authorization (docs/stage-authorization-policy.md §3.2).
/// </summary>
public static class ClaimNames
{
    // The member's ChandaNo, as reported by the member API login response (Data.UserName).
    // Authorization code reads identity ONLY from this claim.
    public const string MembershipNo = "membership_no";

    // The member's display name (e.g. "Azeem Olanrewaju"), used by layouts to greet
    // the user by their real name instead of a hardcoded placeholder.
    public const string FullName = "FullName";

    // One claim instance per role string, as the policy §3.2 requires authorization
    // to read roles from (docs/stage-authorization-policy.md).
    public const string MemberRole = "member_roles";
}
