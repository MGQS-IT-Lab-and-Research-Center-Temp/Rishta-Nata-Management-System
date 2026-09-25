using Application.Authorization;
using Application.DivorceEvidence;
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
    private readonly IDivorceEvidenceService _divorceEvidence;

    public BrideSectionService(
        RishtanataDbContext context,
        IStageAuthorizationService stageAuthorizationService,
        IPartnerEligibilityService eligibility,
        IDivorceEvidenceService divorceEvidence)
    {
        _context = context;
        _stageAuthorizationService = stageAuthorizationService;
        _eligibility = eligibility;
        _divorceEvidence = divorceEvidence;
    }

    public async Task<StageAuthorizationResult> SubmitBrideSectionAsync(
        string membershipNo, Guid applicationFormId, BrideSectionDto dto,
        DivorceEvidenceUpload? divorceEvidence,
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

        // Gap 6: the paper form's marital status is mandatory for the bride.
        // The JsonStringEnumConverter on BrideMaritalStatus still accepts raw
        // integers, so an out-of-range number (e.g. 7) binds successfully to an
        // undefined enum value instead of failing model binding — reject that
        // the same way as a missing value.
        if (dto.BrideMaritalStatus is null || !Enum.IsDefined(dto.BrideMaritalStatus.Value))
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage,
                "Select the bride's marital status.");

        // Gap 8: a divorced bride needs her Khula certificate on file, either
        // uploaded with this submission or kept from an earlier upload on this
        // form. An upload is ignored unless she declares herself divorced.
        var isDivorced = dto.BrideMaritalStatus == BrideMaritalStatus.DivorcedIddatComplete;
        var upload = isDivorced ? divorceEvidence : null;

        if (upload is not null && DivorceEvidenceRules.Validate(upload) is { } uploadError)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage, uploadError);

        var hasEvidence = upload is not null ||
            await _divorceEvidence.HasDocumentAsync(form.Id, DivorceEvidenceParty.Bride, cancellationToken);

        if (isDivorced && !hasEvidence)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage, DivorceEvidenceRules.BrideMissingMessage);

        var eligibility = await _eligibility.ValidateSectionAsync(
            dto.BrideMembershipNo,
            partnerIsGroom: false,
            declaresSubsequentNikah: false,
            isWidower: false,
            isDivorced: false,
            hasDivorceEvidence: false,
            dto.BrideMaritalStatus,
            hasEvidence,
            applicationFormId,
            cancellationToken);

        if (!eligibility.IsAllowed)
        {
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage, eligibility.Message);
        }

        // Store the certificate before touching the section fields. SaveAsync
        // commits, and at this point only the document row is pending.
        if (upload is not null)
            await _divorceEvidence.SaveAsync(
                form.Id, DivorceEvidenceParty.Bride, upload, membershipNo, cancellationToken);

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