using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events;

public  class MarriageFormStageRevertedEvent : DomainEvent
{
    public Guid MarriageFormId { get; }

    public MarriageFormStage PreviousStage { get; }

    public MarriageFormStage CurrentStage { get; }

    public MarriageFormStageRevertedEvent(Guid marriageFormId,MarriageFormStage previousStage,MarriageFormStage currentStage)
    {
        MarriageFormId = marriageFormId;
        PreviousStage = previousStage;
        CurrentStage = currentStage;
    }
}