using Domain.Enums;
using System.Security.Claims;
namespace Application.Interfaces.Auth;

public interface IStageAuthorizationService
{
    Task<StageAuthorizationResult> AuthorizeAsync(Guid marriageApplicationFormId, MarriageFormStage requestedStage, ClaimsPrincipal user,
    CancellationToken cancellationToken = default);
    Task AdvanceStageAsync(Guid marriageApplicationFormId, MarriageFormStage nextStage, CancellationToken cancellationToken = default);
}

public class StageAuthorizationResult
{
    public bool IsAuthorized { get; }
    public string? Reason { get; }

    private StageAuthorizationResult(bool isAuthorized, string? reason)
    {
        IsAuthorized = isAuthorized;
        Reason = reason;
    }

    public static StageAuthorizationResult Allow() => new(true, null);
    public static StageAuthorizationResult Deny(string reason) => new(false, reason);
}