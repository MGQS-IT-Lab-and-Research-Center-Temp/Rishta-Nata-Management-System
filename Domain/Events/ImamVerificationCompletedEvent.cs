using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events;

public  class ImamVerificationCompletedEvent : DomainEvent
{
    public Guid MarriageFormId { get; }

    public ImamVerificationCompletedEvent(Guid marriageFormId)
    {
        MarriageFormId = marriageFormId;
    }
}
