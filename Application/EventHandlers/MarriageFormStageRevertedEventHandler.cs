// File: Application/EventHandlers/MarriageFormStageRevertedEventHandler.cs
using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers;

public class MarriageFormStageRevertedEventHandler : IEventHandler<MarriageFormStageRevertedEvent>
{
    private readonly RishtanataDbContext _context;
    private readonly IMarriageFormNotificationService _notificationService;
    private readonly ILogger<MarriageFormStageRevertedEventHandler> _logger;

    public MarriageFormStageRevertedEventHandler(
        RishtanataDbContext context,
        IMarriageFormNotificationService notificationService,
        ILogger<MarriageFormStageRevertedEventHandler> logger)
    {
        _context = context;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(
        MarriageFormStageRevertedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        // Retrieve the form with all its sections
        var form = await _context.MarriageApplicationForms
            .Include(f => f.BrideSection)
            .Include(f => f.BridegroomSection)
            .Include(f => f.WitnessSignatures)
            .Include(f => f.GuardianOrWakeelSection)
            .Include(f => f.ImamVerification)
            .Include(f => f.JamaatPresidentVerification)
            .Include(f => f.RishtanataRecommendation)
            .Include(f => f.AmirApproval)
            .FirstOrDefaultAsync(f => f.Id == domainEvent.MarriageFormId, cancellationToken);

        if (form == null)
        {
            _logger.LogWarning("Form with ID {FormId} not found for rejection notification", domainEvent.MarriageFormId);
            return;
        }

        // Identify the original submitters based on the reverted stage
        var originalSubmitters = GetOriginalSubmitters(form, domainEvent.CurrentStage);

        if (!originalSubmitters.Any())
        {
            _logger.LogWarning("No original submitters found for form {FormId} at stage {Stage}",
                domainEvent.MarriageFormId, domainEvent.CurrentStage);
            return;
        }

        // Create a rejection record for notification purposes
        var rejection = new MarriageFormRejection
        {
            MarriageApplicationFormId = form.Id,
            RejectedAtStage = (ApplicationStage)domainEvent.PreviousStage,
            RevertedToStage = (ApplicationStage)domainEvent.CurrentStage,
            Reason = domainEvent.Reason
        };

        // Notify each original submitter
        foreach (var submitterId in originalSubmitters)
        {
            rejection.CreatedBy = submitterId; // Set the submitter as the creator for notification purposes
            await _notificationService.NotifyRevertedAsync(form, rejection, cancellationToken);
        }

        _logger.LogInformation(
            "Rejection notification sent to {Count} submitters for form {FormId} reverted from {PreviousStage} to {CurrentStage}",
            originalSubmitters.Count(),
            domainEvent.MarriageFormId,
            domainEvent.PreviousStage,
            domainEvent.CurrentStage);
    }

    private IEnumerable<Guid> GetOriginalSubmitters(
        MarriageApplicationForm form,
        MarriageFormStage revertedStage)
    {
        return revertedStage switch
        {
            MarriageFormStage.AwaitingBride =>
                form.BrideSection?.CreatedBy is Guid brideId ? new[] { brideId } : Enumerable.Empty<Guid>(),
            MarriageFormStage.AwaitingBridegroom =>
                form.BridegroomSection?.CreatedBy is Guid bridegroomId ? new[] { bridegroomId } : Enumerable.Empty<Guid>(),
            MarriageFormStage.AwaitingWitnesses =>
                form.WitnessSignatures.Select(w => w.CreatedBy).Where(id => id.HasValue).Select(id => id.Value).Distinct(),
            MarriageFormStage.AwaitingImamVerification =>
                form.ImamVerification?.CreatedBy is Guid imamId ? new[] { imamId } : Enumerable.Empty<Guid>(),
            MarriageFormStage.AwaitingJamaatPresident =>
                form.JamaatPresidentVerification?.CreatedBy is Guid presidentId ? new[] { presidentId } : Enumerable.Empty<Guid>(),
            MarriageFormStage.AwaitingRishtanataSecretary =>
                form.RishtanataRecommendation?.CreatedBy is Guid secretaryId ? new[] { secretaryId } : Enumerable.Empty<Guid>(),
            MarriageFormStage.AwaitingAmirApproval =>
                form.AmirApproval?.CreatedBy is Guid amirId ? new[] { amirId } : Enumerable.Empty<Guid>(),
            _ => Enumerable.Empty<Guid>()
        };
    }
}
