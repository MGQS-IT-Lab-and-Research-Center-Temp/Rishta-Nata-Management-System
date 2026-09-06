namespace Domain.Constants;

/// <summary>
/// Custom claim type names used for stage authorization (docs/stage-authorization-policy.md §3.2).
/// </summary>
public static class ClaimNames
{
    // The member's ChandaNo, as reported by the member API login response (Data.UserName).
    // Authorization code reads identity ONLY from this claim.
    public const string MembershipNo = "membership_no";
}
