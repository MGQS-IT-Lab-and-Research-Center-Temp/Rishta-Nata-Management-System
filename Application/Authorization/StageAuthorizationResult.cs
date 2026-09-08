namespace Application.Authorization;

/// <summary>
/// Result of a CanUserActAsync check: either allowed, or denied with a
/// machine-readable reason (StageAuthorizationDenyReason) and a human-readable
/// message. Denied = no side effects
/// </summary>
public sealed class StageAuthorizationResult
{
    // True when the user may act on the target stage right now.
    public bool IsAllowed { get; }

    // Why the request was denied; null when allowed.
    public StageAuthorizationDenyReason? Reason { get; }

    // Human-readable detail for logs and debugging.
    public string Message { get; }

    private StageAuthorizationResult(
        bool isAllowed,
        StageAuthorizationDenyReason? reason,
        string message)
    {
        IsAllowed = isAllowed;
        Reason = reason;
        Message = message;
    }

    public static StageAuthorizationResult Allow() =>
        new(true, null, "Allowed.");

    public static StageAuthorizationResult Deny(
        StageAuthorizationDenyReason reason,
        string message) =>
        new(false, reason, message);

    public override string ToString() =>
        IsAllowed ? "Allow" : $"Deny({Reason}: {Message})";
}