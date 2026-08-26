using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Events;

public class MarriageFormStageRevertedEvent : DomainEvent
{
    public Guid MarriageFormId { get; init; }

    public MarriageFormStage PreviousStage { get; init; }

    public MarriageFormStage CurrentStage { get; init; }

    //  Add Reason property to allow the handler to send the reason 
    // without making an additional database query to the MarriageFormRejection table
    public string Reason { get; init; }

    public MarriageFormStageRevertedEvent(Guid marriageFormId, MarriageFormStage previousStage, MarriageFormStage currentStage, string reason)
    {
        MarriageFormId = marriageFormId;
        PreviousStage = previousStage;
        CurrentStage = currentStage;
        Reason = reason;
    }

    // public Guid MarriageFormId { get; }

    // public MarriageFormStage PreviousStage { get; }

    // public MarriageFormStage CurrentStage { get; }

    // public MarriageFormStageRevertedEvent(Guid marriageFormId,MarriageFormStage previousStage,MarriageFormStage currentStage)
    // {
    //     MarriageFormId = marriageFormId;
    //     PreviousStage = previousStage;
    //     CurrentStage = currentStage;
    // }
}