using Application.Interfaces.Auth;
using Domain.Enums;
using Domain.Constants;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services;

public class StageAuthorizationService : IStageAuthorizationService
{
    private readonly RishtanataDbContext _dbContext;
    // Bride/Bridegroom/Witnesses are left unrestricted (any authenticated user)
    private static readonly Dictionary<MarriageFormStage, string[]> StageRoles = new()
    {
        //[MarriageFormStage.AwaitingImamVerification] = [RoleNames.Amir], // TODO: replace with real Imam role
        [MarriageFormStage.AwaitingJamaatPresident] = [RoleNames.JamaatSecretary],
        [MarriageFormStage.AwaitingRishtanataSecretary] = [RoleNames.RishtanataSecretary],
        [MarriageFormStage.AwaitingAmirApproval] = [RoleNames.Amir],
    };

    public StageAuthorizationService(RishtanataDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<StageAuthorizationResult> AuthorizeAsync(
        Guid marriageApplicationFormId,MarriageFormStage requestedStage,ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var currentStage = await _dbContext.MarriageApplicationForms
            .AsNoTracking()
            .Where(f => f.MarriageApplicationId == marriageApplicationFormId)
            .Select(f => (MarriageFormStage?)f.CurrentStage)
            .FirstOrDefaultAsync(cancellationToken);

        if (currentStage is null)
            return StageAuthorizationResult.Deny("Marriage application form not found.");

        if (currentStage != requestedStage)
            return StageAuthorizationResult.Deny($"Form is at stage '{currentStage}', cannot submit '{requestedStage}'.");

        if (StageRoles.TryGetValue(requestedStage, out var allowedRoles) && !allowedRoles.Any(user.IsInRole))
            return StageAuthorizationResult.Deny("User does not have the required role for this stage.");

        return StageAuthorizationResult.Allow();
    }

    public async Task AdvanceStageAsync(Guid marriageApplicationFormId,MarriageFormStage nextStage,CancellationToken cancellationToken = default)
    {
        var updated = await _dbContext.MarriageApplicationForms
            .Where(f => f.MarriageApplicationId == marriageApplicationFormId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(f => f.CurrentStage, nextStage),
                cancellationToken);

        if (updated == 0)
            throw new InvalidOperationException("Marriage application form not found.");
    }
}
