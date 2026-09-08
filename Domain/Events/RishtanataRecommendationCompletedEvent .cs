using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events;

public  class RishtanataRecommendationCompletedEvent : DomainEvent
{
    public Guid MarriageFormId { get; }

    public RishtanataRecommendationCompletedEvent(Guid marriageFormId)
    {
        MarriageFormId = marriageFormId;
    }
}