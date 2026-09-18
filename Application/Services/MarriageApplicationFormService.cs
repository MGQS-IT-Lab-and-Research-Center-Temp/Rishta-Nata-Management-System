using Application.Interfaces;
using Application.Workflow;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.Mapper;

namespace Application.Services;

/// <summary>
/// MarriageApplicationForm CRUD plus the section-signature submissions
/// (guardian/wakeel + witnesses) and the revert/rejection flow.
/// </summary>
public class MarriageApplicationFormService : IMarriageApplicationFormService
{
    private readonly RishtanataDbContext _dbContext;
    private readonly ILogger<MarriageApplicationFormService> _logger;
    private readonly IStageAuthorizationService _stageAuthorization;
    private readonly IMarriageFormNotificationService _notificationService;

    public MarriageApplicationFormService(
        Infrastructure.Persistence.RishtanataDbContext dbContext,
        ILogger<MarriageApplicationFormService> logger,
        IStageAuthorizationService stageAuthorization,
        IMarriageFormNotificationService notificationService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _stageAuthorization = stageAuthorization;
        _notificationService = notificationService;
    }

    // =========================================================
    // CREATE APPLICATION
    // =========================================================

    public async Task<MarriageApplicationForm> CreateAsync(
        MarriageApplicationForm application,
        CancellationToken cancellationToken = default)
    {
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Add(application);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return application;
    }

    public async Task<MarriageApplicationForm> StartApplicationAsync(
        MarriageApplicationForm application,
        string? createdByMembershipNo = null,
        CancellationToken cancellationToken = default)
    {
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        Guid? creatorId = null;
        if (!string.IsNullOrWhiteSpace(createdByMembershipNo))
        {
            creatorId = await _dbContext.JamaatMembers
                .Where(m => m.ChandaNo == createdByMembershipNo)
                .Select(m => (Guid?)m.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // The marriage form is the dependent side of the 1:1 with
        // FormApplication (MarriageApplicationForm.MarriageApplicationId is the
        // FK), so the owning FormApplication must exist first.
        var formApplication = new FormApplication
        {
            Status = ApplicationStatus.Submitted,
            AppliedAt = DateTime.UtcNow,
            CertificateId = null,
            CreatedBy = creatorId
        };

        _dbContext.FormApplications.Add(formApplication);
        await _dbContext.SaveChangesAsync(cancellationToken);

        application.MarriageApplicationId = formApplication.Id;
        application.ReferenceNumber = string.IsNullOrWhiteSpace(application.ReferenceNumber)
            ? GenerateReferenceNumber()
            : application.ReferenceNumber;

        // Only the starting party's section row is created now. The partner's row is
        // created later, when the partner signs in and completes their own section via
        // SubmitBrideSectionAsync / SubmitBridegroomSectionAsync (which upsert-or-create).
        var brideStarted = application.FormStage == MarriageFormStage.AwaitingBridegroom;

        if (brideStarted && application.BrideSection is null && !string.IsNullOrWhiteSpace(application.BrideName))
        {
            application.BrideSection = new BrideFormSection
            {
                BrideMembershipNo = application.BrideMembershipNo,
                BrideName = application.BrideName,
                BrideDateOfBirth = application.BrideDateOfBirth,
                BrideResidentOf = application.BrideResidentOf,
                BrideGenotype = application.BrideGenotype,
                BrideBloodGroup = application.BrideBloodGroup,
                BrideMaritalStatus = application.BrideMaritalStatus,
                BrideDivorceEvidence = application.BrideDivorceEvidence,
                BrideProposedDowerAmount = application.BrideProposedDowerAmount,
                BrideDowerAmountReceivedInCash = application.BrideDowerAmountReceivedInCash,
                BrideSignatureTel = application.BrideSignatureTel,
                ReferenceNumber = application.ReferenceNumber,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
        }

        if (!brideStarted && application.BridegroomSection is null && !string.IsNullOrWhiteSpace(application.BridegroomName))
        {
            application.BridegroomSection = new BridegroomFormSection
            {
                BridegroomMembershipNo = application.BridegroomMembershipNo,
                BridegroomName = application.BridegroomName,
                BridegroomDateOfBirth = application.BridegroomDateOfBirth,
                BridegroomResidentOf = application.BridegroomResidentOf,
                BridegroomGenotype = application.BridegroomGenotype,
                BridegroomBloodGroup = application.BridegroomBloodGroup,
                BridegroomDowerAmountPaidInCash = application.BridegroomDowerAmountPaidInCash,
                BridegroomDowerAmountToBePaid = application.BridegroomDowerAmountToBePaid,
                IsFirstNikah = application.IsFirstNikah,
                CurrentNikahOrdinal = application.CurrentNikahOrdinal,
                FormerWifeIsDead = application.FormerWifeIsDead,
                HasDivorcedFormerWife = application.HasDivorcedFormerWife,
                BridegroomDivorceEvidence = application.BridegroomDivorceEvidence,
                FormerWifeIsPresent = application.FormerWifeIsPresent,
                FormerWifeObtainedKhula = application.FormerWifeObtainedKhula,
                BridegroomSignatureTel = application.BridegroomSignatureTel,
                ReferenceNumber = application.ReferenceNumber,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
        }

        application.CreatedBy = creatorId;
        _dbContext.MarriageApplicationForms.Add(application);
        await _dbContext.SaveChangesAsync(cancellationToken);

        formApplication.MarriageApplicationFormId = application.Id;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return application;
    }

    private static string GenerateReferenceNumber() =>
        $"RN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..16];
    public async Task<ReadOnlyFormDto?> GetReadOnlyFormAsync(
    Guid formId,
    CancellationToken cancellationToken = default)
    {
        var form = await GetByIdAsync(formId, cancellationToken);

        return form == null? null: ReadOnlyFormMapper.MapToReadOnlyDto(form);
    }
    // =========================================================
    // GET BY ID
    // =========================================================

    public async Task<MarriageApplicationForm?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }


    // =========================================================
    // GET BY MARRIAGE APPLICATION ID
    // =========================================================

    public async Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(
        Guid marriageApplicationId)
    {
        return await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.MarriageApplicationId == marriageApplicationId);
    }


    // =========================================================
    // GET BY BRIDEGROOM MEMBERSHIP NUMBER
    // =========================================================

    public async Task<MarriageApplicationForm?> GetByMembershipNoAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(
                x => x.BridegroomMembershipNo == membershipNo,
                cancellationToken);
    }


    // =========================================================
    // UPDATE APPLICATION
    // =========================================================

    public async Task<bool> UpdateAsync(
        MarriageApplicationForm application,
        CancellationToken cancellationToken = default)
    {
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Update(application);

        var affected = await _dbContext.SaveChangesAsync(
            cancellationToken);

        return affected > 0;
    }

    /// <inheritdoc />
    public async Task<RevertStageResult> RevertStageAsync(
        Guid formId,
        ApplicationStage targetStage,
        string reason,
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(membershipNo))
            throw new ArgumentException("A verifier membership number is required.", nameof(membershipNo));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("A rejection reason is required.", nameof(reason));

        var form = await _dbContext.MarriageApplicationForms
            .Include(x => x.MarriageApplication)
            // Accept either the marriage-form id or the wrapping
            // FormApplication.Id (both are used from different review pages).
            .FirstOrDefaultAsync(x => x.Id == formId || x.MarriageApplicationId == formId, cancellationToken);

        if (form is null || !form.ApplicationStage.HasValue)
            return RevertStageResult.FormNotFound;

        if (!Enum.IsDefined(targetStage))
            return RevertStageResult.InvalidTargetStage;

        var currentStage = form.ApplicationStage.Value;
        if (targetStage >= currentStage )
            return RevertStageResult.InvalidTargetStage;
        if (form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
            return RevertStageResult.ApplicationAlreadyApproved; 

        var authorization = await _stageAuthorization.CanUserActAsync(
            membershipNo, form.Id, currentStage, cancellationToken);

        if (!authorization.IsAllowed)
            return RevertStageResult.Unauthorized;

        var verifierId = await _dbContext.JamaatMembers
            .Where(m => m.ChandaNo == membershipNo)
            .Select(m => (Guid?)m.Id)
            .FirstOrDefaultAsync(cancellationToken) ?? Guid.Empty;

        var rejection = new MarriageFormRejection
        {
            MarriageApplicationFormId = form.Id,
            RejectedAtStage = currentStage,
            RevertedToStage = targetStage,
            Reason = reason.Trim(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = verifierId
        };

        await ClearSectionsAfterAsync(form.Id, targetStage, cancellationToken);
        form.ApplicationStage = targetStage;
        form.FormStage = WorkflowStageMapping.ToFormStage(targetStage);
        _dbContext.MarriageFormRejections.Add(rejection);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _notificationService.NotifyRevertedAsync(form, rejection, cancellationToken);
        return RevertStageResult.Success;
    }

    private async Task ClearSectionsAfterAsync(
        Guid formId,
        ApplicationStage targetStage,
        CancellationToken cancellationToken)
    {
        if (targetStage == ApplicationStage.ApplicantsReview)
        {
            await RemoveSectionsAsync<GuardianOrWakeelSection>(formId, cancellationToken);
            await RemoveSectionsAsync<ImamVerificationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<JamaatPresidentVerificationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<GroomJamaatPresidentVerificationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<RishtanataRecommendationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<AmirApprovalSection>(formId, cancellationToken);
            await RemoveSectionsAsync<WitnessSignatureSection>(formId, cancellationToken);
        }
        else if (targetStage == ApplicationStage.JamaatPresidentReview)
        {
            // The groom-president step (when it happened) and everything after the
            // president chain must be redone; the imam sign-off is forward too.
            await RemoveSectionsAsync<GroomJamaatPresidentVerificationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<RishtanataRecommendationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<AmirApprovalSection>(formId, cancellationToken);
            await RemoveSectionsAsync<ImamVerificationSection>(formId, cancellationToken);
        }
        else if (targetStage == ApplicationStage.NationalRishtanataSecretaryVerification)
        {
            await RemoveSectionsAsync<AmirApprovalSection>(formId, cancellationToken);
            await RemoveSectionsAsync<ImamVerificationSection>(formId, cancellationToken);
        }
        else if (targetStage == ApplicationStage.AmirApproval)
        {
            await RemoveSectionsAsync<ImamVerificationSection>(formId, cancellationToken);
        }
    }

    private async Task RemoveSectionsAsync<TEntity>(Guid formId, CancellationToken cancellationToken)
        where TEntity : class
    {
        var sections = await _dbContext.Set<TEntity>()
            .Where(entity => EF.Property<Guid>(entity, "MarriageApplicationFormId") == formId)
            .ToListAsync(cancellationToken);

        _dbContext.RemoveRange(sections);
    }
    
    // =========================================================
    // GUARDIAN / WAKEEL SIGNATURE
    // =========================================================

    public async Task<bool> SubmitGuardianOrWakeelAsync(
        Guid marriageApplicationFormId,
        string signature,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        var application = await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.Id == marriageApplicationFormId,
                cancellationToken);

        if (application == null)
        {
            _logger.LogWarning(
                "Marriage application {ApplicationId} was not found.",
                marriageApplicationFormId);

            return false;
        }

        // Make sure Guardian/Wakeel section exists
        if (application.GuardianOrWakeelSection == null)
        {
            _logger.LogWarning(
                "Guardian/Wakeel section not found for application {ApplicationId}.",
                marriageApplicationFormId);

            return false;
        }

        // Save signature
        application.GuardianOrWakeelSection.Signature = signature;
        application.GuardianOrWakeelSection.Date = DateTime.UtcNow;

        // Check whether guardian/wakeel has signed
        bool guardianOrWakeelSigned =
            !string.IsNullOrWhiteSpace(
                application.GuardianOrWakeelSection.Signature);

        // Check whether both witnesses have signed
        bool bothWitnessesSigned =
            application.WitnessSignatures.Count >= 2 &&
            application.WitnessSignatures.All(
                w => !string.IsNullOrWhiteSpace(w.Signature));

        // Only advance when EVERYONE has signed
        if (guardianOrWakeelSigned && bothWitnessesSigned)
        {
            application.FormStage =
                MarriageFormStage.AwaitingBrideJamaatPresident;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }


    // =========================================================
    // WITNESS SIGNATURE
    // =========================================================

    public async Task<bool> SubmitWitnessSignatureAsync(
        Guid marriageApplicationFormId,
        Guid witnessSignatureId,
        string signature,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        var application = await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.Id == marriageApplicationFormId,
                cancellationToken);

        if (application == null)
        {
            _logger.LogWarning(
                "Marriage application {ApplicationId} was not found.",
                marriageApplicationFormId);

            return false;
        }

        // Find the specific witness
        var witness = application.WitnessSignatures
            .FirstOrDefault(
                w => w.Id == witnessSignatureId);

        if (witness == null)
        {
            _logger.LogWarning(
                "Witness {WitnessId} was not found for application {ApplicationId}.",
                witnessSignatureId,
                marriageApplicationFormId);

            return false;
        }

        // Save witness signature
        witness.Signature = signature;
        witness.SignatureDate = DateTime.UtcNow;

        // Check guardian/wakeel
        bool guardianOrWakeelSigned =
            application.GuardianOrWakeelSection != null &&
            !string.IsNullOrWhiteSpace(
                application.GuardianOrWakeelSection.Signature);

        // Check both witnesses
        bool bothWitnessesSigned =
            application.WitnessSignatures.Count >= 2 &&
            application.WitnessSignatures.All(
                w => !string.IsNullOrWhiteSpace(w.Signature));

        // Advance only when guardian/wakeel AND both witnesses signed
        if (guardianOrWakeelSigned && bothWitnessesSigned)
        {
            application.FormStage =
                MarriageFormStage.AwaitingBrideJamaatPresident;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}