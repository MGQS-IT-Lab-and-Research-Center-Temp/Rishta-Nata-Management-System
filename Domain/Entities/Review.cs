using System;
using Domain.Abstractions;

namespace Domain.Entities;

public class Review : AuditableEntity
{
    // The MarriageApplicationForm being reviewed. One form can have many reviews
    // (e.g. President Review, Secretary Review, future review stages).
    public Guid MarriageApplicationFormId { get; set; }
    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

    // Who performed this review (Identity user Guid).
    public Guid ReviewerId { get; set; }

    // What this reviewer decided for this stage (e.g. "Approved", "Rejected", "InformationRequired").
    // Deliberately separate from ApplicationStatus, which tracks the overall application state.
    public string Status { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public DateTime ReviewedAt { get; set; }
}