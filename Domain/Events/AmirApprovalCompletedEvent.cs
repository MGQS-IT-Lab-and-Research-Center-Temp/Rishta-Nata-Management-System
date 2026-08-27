using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events;

public  class AmirApprovalCompletedEvent : DomainEvent
{
    public Guid MarriageFormId { get; }

    public AmirApprovalCompletedEvent(Guid marriageFormId)
    {
        MarriageFormId = marriageFormId;
    }
}
