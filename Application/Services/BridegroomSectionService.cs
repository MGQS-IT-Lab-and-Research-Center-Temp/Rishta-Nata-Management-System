using Application.Authorization;
using Application.Dower;
using Application.DivorceEvidence;
using Application.Interfaces;
using Domain.Entities;
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
    private readonly IPartnerEligibilityService _eligibility;
    private readonly IDivorceEvidenceService _divorceEvidence;

    public BridegroomSectionService(
        RishtanataDbContext dbContext,
        IStageAuthorizationService stageAuthorizationService,
        IPartnerEligibilityService eligibility,
        IDivorceEvidenceService divorceEvidence)
    {
        _dbContext = dbContext;
        _stageAuthorizationService = stageAuthorizationService;
        _eligibility = eligibility;
        _divorceEvidence = divorceEvidence;
    }

    public async Task<StageAuthorizationResult> SubmitBridegroomSectionAsync(
        string membershipNo, Guid applicationFormId, BridegroomSectionDto dto,
        DivorceEvidenceUpload? divorceEvidence,
        CancellationToken cancellationToken = default)
    {
        var authResult = await _stageAuthorizationService.CanUserActAsync(
            membershipNo, applicationFormId, ApplicationStage.ApplicantsReview, cancellationToken);

        if (!authResult.IsAllowed)
            return authResult;

        var form = await _dbContext.MarriageApplicationForms
            .Include(x => x.BridegroomSection)
            .Include(x => x.GroomWakeelSection)
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId || f.MarriageApplicationId == applicationFormId,
                cancellationToken);

        if (form is null)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists.");

        // Re-check the granular intake state before writing — the form must
        // actually still be waiting on the bridegroom. Either party may start:
        //  - AwaitingApplicants   => bridegroom is first, advance to AwaitingBride
        //  - AwaitingBridegroom   => bride already submitted, advance to witnesses
        var nextStage = form.FormStage switch
        {
            MarriageFormStage.AwaitingApplicants => MarriageFormStage.AwaitingBride,
            MarriageFormStage.AwaitingBridegroom => MarriageFormStage.AwaitingWitnesses,
            _ => (MarriageFormStage?)null
        };

        if (nextStage is null)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage,
                $"Form is at {form.FormStage}, not awaiting the bridegroom.");

        // Gap 9: paid in cash + still to be paid must equal the total dower.
        var dowerError = BridegroomDowerRules.Validate(
            dto.BridegroomDowerAmountPaidInCash,
            dto.BridegroomDowerAmountToBePaid,
            dto.BridegroomTotalDowerAmount);

        if (dowerError is not null)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage, dowerError);

        // Gap 8: a groom who divorced a former wife needs the Talaq certificate
        // on file, either uploaded with this submission or kept from an earlier
        // upload on this form. An upload is ignored unless he declares the divorce.
        var isDivorced = dto.HasDivorcedFormerWife;
        var upload = isDivorced ? divorceEvidence : null;

        if (upload is not null && DivorceEvidenceRules.Validate(upload) is { } uploadError)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage, uploadError);

        var hasEvidence = upload is not null ||
            await _divorceEvidence.HasDocumentAsync(form.Id, DivorceEvidenceParty.Bridegroom, cancellationToken);

        if (isDivorced && !hasEvidence)
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage, DivorceEvidenceRules.GroomMissingMessage);

        var eligibility = await _eligibility.ValidateSectionAsync(
            dto.BridegroomMembershipNo,
            partnerIsGroom: true,
            dto.CurrentNikahOrdinal is not null,
            dto.FormerWifeIsDead,
            dto.HasDivorcedFormerWife,
            hasEvidence,
            brideMaritalStatus: null,
            brideHasDivorceEvidence: false,
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
                form.Id, DivorceEvidenceParty.Bridegroom, upload, membershipNo, cancellationToken);

        // Persist the bridegroom's section fields onto the form
        form.BridegroomMembershipNo = dto.BridegroomMembershipNo;
        form.BridegroomName = dto.BridegroomName;
        form.BridegroomDateOfBirth = dto.BridegroomDateOfBirth;
        form.BridegroomResidentOf = dto.BridegroomResidentOf;
        form.BridegroomGenotype = dto.BridegroomGenotype;
        form.BridegroomBloodGroup = dto.BridegroomBloodGroup;
        form.BridegroomDowerAmountPaidInCash = dto.BridegroomDowerAmountPaidInCash;
        form.BridegroomDowerAmountToBePaid = dto.BridegroomDowerAmountToBePaid;
        form.BridegroomTotalDowerAmount = dto.BridegroomTotalDowerAmount;
        form.IsFirstNikah = dto.IsFirstNikah;
        form.CurrentNikahOrdinal = dto.CurrentNikahOrdinal;
        form.FormerWifeIsDead = dto.FormerWifeIsDead;
        form.HasDivorcedFormerWife = dto.HasDivorcedFormerWife;
        form.FormerWifeIsPresent = dto.FormerWifeIsPresent;
        form.FormerWifeObtainedKhula = dto.FormerWifeObtainedKhula;
        form.BridegroomSignatureTel = dto.BridegroomSignatureTel;
        form.BridegroomDivorceEvidence = dto.BridegroomDivorceEvidence;

        var wakeelName = dto.CanAttendNikahInPerson ? string.Empty : dto.WakeelName?.Trim() ?? string.Empty;
        var wakeelTel = dto.CanAttendNikahInPerson ? string.Empty : dto.WakeelTel?.Trim() ?? string.Empty;

        form.CanAttendNikahInPerson = dto.CanAttendNikahInPerson;
        form.WakeelName = wakeelName;
        form.WakeelTel = wakeelTel;

        // Authoritative per-party store, kept in parity with the flat mirror.
        var bridegroomSection = form.BridegroomSection ??= new BridegroomFormSection
        {
            ReferenceNumber = form.ReferenceNumber,
            CreatedAt = DateTime.UtcNow
        };

        bridegroomSection.ReferenceNumber = form.ReferenceNumber;
        bridegroomSection.BridegroomMembershipNo = dto.BridegroomMembershipNo;
        bridegroomSection.BridegroomName = dto.BridegroomName;
        bridegroomSection.BridegroomDateOfBirth = dto.BridegroomDateOfBirth;
        bridegroomSection.BridegroomResidentOf = dto.BridegroomResidentOf;
        bridegroomSection.BridegroomGenotype = dto.BridegroomGenotype;
        bridegroomSection.BridegroomBloodGroup = dto.BridegroomBloodGroup;
        bridegroomSection.BridegroomDowerAmountPaidInCash = dto.BridegroomDowerAmountPaidInCash;
        bridegroomSection.BridegroomDowerAmountToBePaid = dto.BridegroomDowerAmountToBePaid;
        bridegroomSection.BridegroomTotalDowerAmount = dto.BridegroomTotalDowerAmount;
        bridegroomSection.IsFirstNikah = dto.IsFirstNikah;
        bridegroomSection.CurrentNikahOrdinal = dto.CurrentNikahOrdinal;
        bridegroomSection.FormerWifeIsDead = dto.FormerWifeIsDead;
        bridegroomSection.HasDivorcedFormerWife = dto.HasDivorcedFormerWife;
        bridegroomSection.FormerWifeIsPresent = dto.FormerWifeIsPresent;
        bridegroomSection.FormerWifeObtainedKhula = dto.FormerWifeObtainedKhula;
        bridegroomSection.BridegroomSignatureTel = dto.BridegroomSignatureTel;
        bridegroomSection.BridegroomDivorceEvidence = dto.BridegroomDivorceEvidence;
        bridegroomSection.CanAttendNikahInPerson = dto.CanAttendNikahInPerson;
        bridegroomSection.WakeelName = wakeelName;
        bridegroomSection.WakeelTel = wakeelTel;
        bridegroomSection.ModifiedAt = DateTime.UtcNow;

        if (!dto.CanAttendNikahInPerson && !string.IsNullOrWhiteSpace(wakeelName))
        {
            var wakeelSection = form.GroomWakeelSection ??= new GroomWakeelSection
            {
                ReferenceNumber = form.ReferenceNumber,
                CreatedAt = DateTime.UtcNow
            };
            wakeelSection.Name = wakeelName;
            wakeelSection.Tel = wakeelTel;
            wakeelSection.ModifiedAt = DateTime.UtcNow;

            form.GroomWakeelName = wakeelName;
            form.GroomWakeelTel = wakeelTel;
        }
        else if (dto.CanAttendNikahInPerson)
        {
            await ClearGroomWakeelAsync(form, cancellationToken);
        }

        form.FormStage = nextStage.Value;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return StageAuthorizationResult.Allow();
    }

    /// <summary>
    /// The groom now attends in person: drop any Wakeel appointment (section row
    /// and flat mirrors) and kill any GroomWakeel link, the same way
    /// SharedSectionService releases a withdrawn representative's links.
    /// Tracked changes only; the caller saves.
    /// </summary>
    private async Task ClearGroomWakeelAsync(MarriageApplicationForm form, CancellationToken cancellationToken)
    {
        if (form.GroomWakeelSection is not null)
        {
            _dbContext.Remove(form.GroomWakeelSection);
            form.GroomWakeelSection = null;
        }

        form.GroomWakeelName = string.Empty;
        form.GroomWakeelFatherName = string.Empty;
        form.GroomWakeelTel = string.Empty;
        form.GroomWakeelSignatureDate = string.Empty;

        var tokens = await _dbContext.SectionAccessTokens
            .Where(x => x.MarriageApplicationFormId == form.Id && x.SectionType == SectionType.GroomWakeel)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.RevokedAt ??= DateTime.UtcNow;
            token.SubmittedAt = null;
            token.RawToken = string.Empty;
            token.TokenHash = string.Empty;
            token.ModifiedAt = DateTime.UtcNow;
        }
    }
}