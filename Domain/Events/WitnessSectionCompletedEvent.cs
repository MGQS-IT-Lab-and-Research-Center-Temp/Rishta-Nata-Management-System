using Domain.Abstractions;
namespace Domain.Events;

public  class WitnessSectionCompletedEvent : DomainEvent
{
    public Guid MarriageFormId { get; }

    public WitnessSectionCompletedEvent(Guid marriageFormId)
    {
        MarriageFormId = marriageFormId;
    }
}