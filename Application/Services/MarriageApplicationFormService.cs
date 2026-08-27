using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
// using Infrastructure.DTOs.ReadOnlyFormDto;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

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
        Guid verifierId,
        CancellationToken cancellationToken = default)
    {
        if (verifierId == Guid.Empty)
            throw new ArgumentException("A verifier id is required.", nameof(verifierId));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("A rejection reason is required.", nameof(reason));

        var form = await _dbContext.MarriageApplicationForms
            .Include(x => x.MarriageApplication)
            .FirstOrDefaultAsync(x => x.Id == formId, cancellationToken);

        if (form is null || !form.ApplicationStage.HasValue)
            return RevertStageResult.FormNotFound;

        var currentStage = form.ApplicationStage.Value;
        if (targetStage >= currentStage )
            return RevertStageResult.InvalidTargetStage;
        if (form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
            return RevertStageResult.ApplicationAlreadyApproved; 

        var authorization = await _stageAuthorization.CanUserActAsync(
            verifierId, form.Id, currentStage, cancellationToken);

        if (!authorization.IsAllowed)
            return RevertStageResult.Unauthorized;

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
            await RemoveSectionsAsync<RishtanataRecommendationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<AmirApprovalSection>(formId, cancellationToken);
            await RemoveSectionsAsync<WitnessSignatureSection>(formId, cancellationToken);
        }
        else if (targetStage == ApplicationStage.JamaatPresidentReview)
        {
            await RemoveSectionsAsync<RishtanataRecommendationSection>(formId, cancellationToken);
            await RemoveSectionsAsync<AmirApprovalSection>(formId, cancellationToken);
        }
        else if (targetStage == ApplicationStage.NationalRishtanataSecretaryVerification)
        {
            await RemoveSectionsAsync<AmirApprovalSection>(formId, cancellationToken);
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
                MarriageFormStage.AwaitingImamVerification;
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
                MarriageFormStage.AwaitingImamVerification;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}