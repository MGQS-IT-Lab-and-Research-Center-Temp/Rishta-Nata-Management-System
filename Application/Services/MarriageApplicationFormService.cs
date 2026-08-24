using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Infrastructure.Persistence; 
using Domain.Enums;
using Domain.Interfaces;
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

    public async Task<MarriageApplicationForm?> GetByMembershipNoAsync(string membershipNo, CancellationToken ct = default)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(f => f.BridegroomMembershipNo == membershipNo, ct);
    }

    public async Task<MarriageApplicationForm> CreateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default)
    {
        if (application is null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Add(application);

        var saved = await _dbContext.SaveChangesAsync(cancellationToken);
        return application;
    }

    public async Task<MarriageApplicationForm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(Guid marriageAplicationId)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(x => x.MarriageApplicationId == marriageAplicationId);
    }

    public async Task<bool> UpdateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default)
    {
        if (application is null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Update(application);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);
        return affected > 0;
    }

    /// <inheritdoc />
    public async Task<bool> RevertStageAsync(
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
            return false;

        var currentStage = form.ApplicationStage.Value;
        if (targetStage >= currentStage ||
            form.MarriageApplication?.Status == ApplicationStatus.ApplicationApproved)
            return false;

        var authorization = await _stageAuthorization.CanUserActAsync(
            verifierId, form.Id, currentStage, cancellationToken);

        if (!authorization.IsAllowed)
            return false;

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
        return true;
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
}
