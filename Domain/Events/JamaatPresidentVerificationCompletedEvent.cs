using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events;

public class JamaatPresidentVerificationCompletedEvent : DomainEvent
{
    public Guid MarriageFormId { get; }

    public JamaatPresidentVerificationCompletedEvent(Guid marriageFormId)
    {
        MarriageFormId = marriageFormId;
    }
}
