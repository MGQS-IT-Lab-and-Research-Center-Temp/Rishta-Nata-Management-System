using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events;

public  class BridegroomSectionCompletedEvent : DomainEvent
{
    public Guid MarriageFormId { get; }

    public BridegroomSectionCompletedEvent(Guid marriageFormId)
    {
        MarriageFormId = marriageFormId;
    }
}