using Application.Authorization;
using Application.Interfaces;
using Domain.Entities;
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
    private readonly IPartnerEligibilityService _eligibility;

    public BrideSectionService(
        RishtanataDbContext context,
        IStageAuthorizationService stageAuthorizationService,
        IPartnerEligibilityService eligibility)
    {
        _context = context;
        _stageAuthorizationService = stageAuthorizationService;
        _eligibility = eligibility;
    }

    public async Task<StageAuthorizationResult> SubmitBrideSectionAsync(
        string membershipNo, Guid applicationFormId, BrideSectionDto dto,
        CancellationToken cancellationToken = default)
    {
        var authResult = await _stageAuthorizationService.CanUserActAsync(
            membershipNo, applicationFormId, ApplicationStage.ApplicantsReview, cancellationToken);

        if (!authResult.IsAllowed)
            return authResult;

        var form = await _context.MarriageApplicationForms
            .Include(x => x.BrideSection)
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId || f.MarriageApplicationId == applicationFormId,
                cancellationToken);

        if (form is null)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");

        // Re-check the granular intake state before writing — a role/identity
        // match at ApplicantsReview isn't enough on its own; the form must
        // actually still be waiting on the bride. Either party may start:
        //  - AwaitingApplicants => bride is first, advance to AwaitingBridegroom
        //  - AwaitingBride       => bridegroom already submitted, advance to witnesses
        var nextStage = form.FormStage switch
        {
            MarriageFormStage.AwaitingApplicants => MarriageFormStage.AwaitingBridegroom,
            MarriageFormStage.AwaitingBride => MarriageFormStage.AwaitingWitnesses,
            _ => (MarriageFormStage?)null
        };

        if (nextStage is null)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage,
                $"Form is at {form.FormStage}, not awaiting the bride.");

        var eligibility = await _eligibility.ValidateSectionAsync(
            dto.BrideMembershipNo,
            partnerIsGroom: false,
            declaresSubsequentNikah: false,
            isWidower: false,
            isDivorced: false,
            divorceEvidence: string.Empty,
            dto.BrideMaritalStatus,
            dto.BrideDivorceEvidence,
            applicationFormId,
            cancellationToken);

        if (!eligibility.IsAllowed)
        {
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage, eligibility.Message);
        }

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
        form.BrideDivorceEvidence = dto.BrideDivorceEvidence;

        // Authoritative per-party store, kept in parity with the flat mirror.
        var brideSection = form.BrideSection ??= new BrideFormSection
        {
            ReferenceNumber = form.ReferenceNumber,
            CreatedAt = DateTime.UtcNow
        };

        brideSection.ReferenceNumber = form.ReferenceNumber;
        brideSection.BrideMembershipNo = dto.BrideMembershipNo;
        brideSection.BrideName = dto.BrideName;
        brideSection.BrideDateOfBirth = dto.BrideDateOfBirth;
        brideSection.BrideResidentOf = dto.BrideResidentOf;
        brideSection.BrideGenotype = dto.BrideGenotype;
        brideSection.BrideBloodGroup = dto.BrideBloodGroup;
        brideSection.BrideMaritalStatus = dto.BrideMaritalStatus;
        brideSection.BrideProposedDowerAmount = dto.BrideProposedDowerAmount;
        brideSection.BrideDowerAmountReceivedInCash = dto.BrideDowerAmountReceivedInCash;
        brideSection.BrideSignatureTel = dto.BrideSignatureTel;
        brideSection.BrideDivorceEvidence = dto.BrideDivorceEvidence;
        brideSection.ModifiedAt = DateTime.UtcNow;

        form.FormStage = nextStage.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return StageAuthorizationResult.Allow();
    }
}