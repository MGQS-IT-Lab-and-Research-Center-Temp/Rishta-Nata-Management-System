using Application.Authorization;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.DTOs.BrideGroom;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Stage-gated submission of the bridegroom's section onto the marriage form.
/// Split off BridegroomService (cleanup) so the staged-submission path and the
/// BridegroomFormSection record CRUD are separate single-responsibility
/// services; the controller that used to call the combined service now uses
/// this one.
/// </summary>
public class BridegroomSectionService : IBridegroomSectionService
{
    private readonly RishtanataDbContext _dbContext;
    private readonly IStageAuthorizationService _stageAuthorizationService;

    public BridegroomSectionService(
        RishtanataDbContext dbContext,
        IStageAuthorizationService stageAuthorizationService)
    {
        _dbContext = dbContext;
        _stageAuthorizationService = stageAuthorizationService;
    }

    public async Task<StageAuthorizationResult> SubmitBridegroomSectionAsync(
        Guid userId, Guid applicationFormId, BridegroomSectionDto dto,
        CancellationToken cancellationToken = default)
    {
        var authResult = await _stageAuthorizationService.CanUserActAsync(
            userId, applicationFormId, ApplicationStage.ApplicantsReview, cancellationToken);

        if (!authResult.IsAllowed)
            return authResult;

        var form = await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId || f.MarriageApplicationId == applicationFormId,
                cancellationToken);

        if (form is null)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");

        // Re-check the granular intake state before writing — the form must
        // actually still be waiting on the bridegroom specifically.
        if (form.FormStage != MarriageFormStage.AwaitingBridegroom)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage,
                $"Form is at {form.FormStage}, not AwaitingBridegroom.");

        // Persist the bridegroom's section fields onto the form
        form.BridegroomMembershipNo = dto.BridegroomMembershipNo;
        form.BridegroomName = dto.BridegroomName;
        form.BridegroomDateOfBirth = dto.BridegroomDateOfBirth;
        form.BridegroomResidentOf = dto.BridegroomResidentOf;
        form.BridegroomGenotype = dto.BridegroomGenotype;
        form.BridegroomBloodGroup = dto.BridegroomBloodGroup;
        form.BridegroomDowerAmountPaidInCash = dto.BridegroomDowerAmountPaidInCash;
        form.BridegroomDowerAmountToBePaid = dto.BridegroomDowerAmountToBePaid;
        form.IsFirstNikah = dto.IsFirstNikah;
        form.IsSecondThirdOrFourthNikah = dto.IsSecondThirdOrFourthNikah;
        form.FormerWifeIsDead = dto.FormerWifeIsDead;
        form.HasDivorcedFormerWife = dto.HasDivorcedFormerWife;
        form.FormerWifeIsPresent = dto.FormerWifeIsPresent;
        form.FormerWifeObtainedKhula = dto.FormerWifeObtainedKhula;
        form.BridegroomSignatureTel = dto.BridegroomSignatureTel;

        form.FormStage = MarriageFormStage.AwaitingWitnesses;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return StageAuthorizationResult.Allow();
    }
}