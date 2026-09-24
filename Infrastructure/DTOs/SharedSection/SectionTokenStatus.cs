using Domain.Enums;

namespace Infrastructure.DTOs.SharedSection;

public class SectionTokenStatus
{
    public bool IsValid { get; init; }
    public Guid FormId { get; init; }
    public SectionType SectionType { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public string BrideName { get; init; } = string.Empty;
    public string BridegroomName { get; init; } = string.Empty;
    public string InvalidationReason { get; init; } = string.Empty;
    public bool IsSubmitted { get; init; }

    /// <summary>Saved guardian choice; prefills the Fill form for SectionType.Guardian. False for other sections.</summary>
    public bool AppointsRepresentative { get; set; }
}