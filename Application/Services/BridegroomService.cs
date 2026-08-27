using Application.Authorization;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.BrideGroom;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BridegroomService : IBridegroomService
{
    private readonly RishtanataDbContext _dbContext;
    private readonly IStageAuthorizationService _stageAuthorizationService;

    public BridegroomService(
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

    public async Task<BridegroomFormSection> CreateOrUpdateAsync(
        BridegroomFormSection bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        var existingBridegroom = await GetByMembershipNoAsync(
            bridegroom.BridegroomMembershipNo,
            cancellationToken);

        if (existingBridegroom is null)
        {
            return await CreateAsync(bridegroom, cancellationToken);
        }

        existingBridegroom.BridegroomName = bridegroom.BridegroomName;
        existingBridegroom.BridegroomDateOfBirth = bridegroom.BridegroomDateOfBirth;
        existingBridegroom.BridegroomResidentOf = bridegroom.BridegroomResidentOf;
        existingBridegroom.BridegroomGenotype = bridegroom.BridegroomGenotype;
        existingBridegroom.BridegroomBloodGroup = bridegroom.BridegroomBloodGroup;
        existingBridegroom.BridegroomDowerAmountPaidInCash = bridegroom.BridegroomDowerAmountPaidInCash;
        existingBridegroom.BridegroomDowerAmountToBePaid = bridegroom.BridegroomDowerAmountToBePaid;
        existingBridegroom.BridegroomSignatureTel = bridegroom.BridegroomSignatureTel;
        existingBridegroom.IsFirstNikah = bridegroom.IsFirstNikah;
        existingBridegroom.IsSecondThirdOrFourthNikah = bridegroom.IsSecondThirdOrFourthNikah;
        existingBridegroom.FormerWifeIsDead = bridegroom.FormerWifeIsDead;
        existingBridegroom.HasDivorcedFormerWife = bridegroom.HasDivorcedFormerWife;
        existingBridegroom.FormerWifeIsPresent = bridegroom.FormerWifeIsPresent;
        existingBridegroom.FormerWifeObtainedKhula = bridegroom.FormerWifeObtainedKhula;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return existingBridegroom;
    }

    public async Task<BridegroomFormSection> CreateAsync(
        BridegroomFormSection bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        _dbContext.BridegroomFormSections.Add(bridegroom);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return bridegroom;
    }

    public async Task<BridegroomFormSection?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.BridegroomFormSections
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<BridegroomFormSection?> GetByMembershipNoAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.BridegroomFormSections
            .FirstOrDefaultAsync(
                x => x.BridegroomMembershipNo == membershipNo,
                cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        BridegroomFormSection bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        _dbContext.BridegroomFormSections.Update(bridegroom);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);

        return affected > 0;
    }
}