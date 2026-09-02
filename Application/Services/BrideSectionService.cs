using Application.Authorization;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// Stage-gated submission of the bride's section onto the marriage form.
/// Split off BrideGuardianService (cleanup) so the staged-submission path and
/// the BrideGuardian record CRUD are separate single-responsibility services;
/// the controller that used to call the combined service now uses this one.
/// </summary>
public class BrideSectionService : IBrideSectionService
{
    private readonly RishtanataDbContext _context;
    private readonly IStageAuthorizationService _stageAuthorizationService;

    public BrideSectionService(
        RishtanataDbContext context,
        IStageAuthorizationService stageAuthorizationService)
    {
        _context = context;
        _stageAuthorizationService = stageAuthorizationService;
    }

    public async Task<StageAuthorizationResult> SubmitBrideSectionAsync(
        Guid userId, Guid applicationFormId, BrideSectionDto dto,
        CancellationToken cancellationToken = default)
    {
        var authResult = await _stageAuthorizationService.CanUserActAsync(
            userId, applicationFormId, ApplicationStage.ApplicantsReview, cancellationToken);

        if (!authResult.IsAllowed)
            return authResult;

        var form = await _context.MarriageApplicationForms
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId || f.MarriageApplicationId == applicationFormId,
                cancellationToken);

        if (form is null)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");

        // Re-check the granular intake state before writing — a role/identity
        // match at ApplicantsReview isn't enough on its own; the form must
        // actually still be waiting on the bride specifically.
        if (form.FormStage != MarriageFormStage.AwaitingBride)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage,
                $"Form is at {form.FormStage}, not AwaitingBride.");

        // Persist the bride's section fields onto the form
        form.BrideMembershipNo = dto.BrideMembershipNo;
        form.BrideName = dto.BrideName;
        form.BrideDateOfBirth = dto.BrideDateOfBirth;
        form.BrideResidentOf = dto.BrideResidentOf;
        form.BrideGenotype = dto.BrideGenotype;
        form.BrideBloodGroup = dto.BrideBloodGroup;
        form.BrideMaritalStatus = dto.BrideMaritalStatus;
        form.BrideProposedDowerAmount = dto.BrideProposedDowerAmount;
        form.BrideDowerAmountReceivedInCash = dto.BrideDowerAmountReceivedInCash;
        form.BrideSignatureTel = dto.BrideSignatureTel;

        form.FormStage = MarriageFormStage.AwaitingBridegroom;

        await _context.SaveChangesAsync(cancellationToken);
        return StageAuthorizationResult.Allow();
    }
}