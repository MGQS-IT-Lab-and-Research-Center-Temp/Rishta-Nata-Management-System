using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events
{
    public class BrideSectionCompletedEvent : DomainEvent
    {
        public Guid MarriageFormId { get; }

        public BrideSectionCompletedEvent(Guid marriageFormId)
        {
            MarriageFormId = marriageFormId;
        }
    }
}
