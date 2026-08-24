using Domain.Events;
using Domain.Abstractions;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EventHandlers;

public class MarriageFormStageRevertedEventHandler : IEventHandler<MarriageFormStageRevertedEvent>
{
    private readonly IMarriageFormRepository _formRepository;
    private readonly INotificationDispatcher _notificationDispatcher;

    public MarriageFormStageRevertedEventHandler(
        IMarriageFormRepository formRepository,
        INotificationDispatcher notificationDispatcher)
    {
        _formRepository = formRepository;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task Handle(
        MarriageFormStageRevertedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var form = await _formRepository.GetByIdWithSectionsAsync(domainEvent.MarriageFormId);
        if (form == null) return;

        var originalSubmitters = GetOriginalSubmitters(form, domainEvent.CurrentStage);

        foreach (var submitterId in originalSubmitters)
        {
            await _notificationDispatcher.DispatchRejectionNotificationAsync(
                submitterId,
                domainEvent.MarriageFormId,
                domainEvent.Reason
            );
        }
    }

    private IEnumerable<Guid> GetOriginalSubmitters(
        MarriageApplicationForm form,
        MarriageFormStage revertedStage)
    {
        return revertedStage switch
        {
            MarriageFormStage.AwaitingBride =>
                new[] { form.BrideSection?.CreatedBy }.Where(id => id.HasValue).Select(id => id.Value),
            MarriageFormStage.AwaitingBridegroom =>
                new[] { form.BridegroomSection?.CreatedBy }.Where(id => id.HasValue).Select(id => id.Value),
            MarriageFormStage.AwaitingWitnesses =>
                form.WitnessSignatures.Select(w => w.CreatedBy).Distinct(),
            // Add other cases as needed
            _ => Enumerable.Empty<Guid>()
        };
    }
}
