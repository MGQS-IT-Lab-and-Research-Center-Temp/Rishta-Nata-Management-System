using Domain.Enums;

namespace Infrastructure.DTOs.SharedSection;

public class SectionLinkStatus
{
    public SectionType Section { get; init; }
    public bool HasActiveToken { get; init; }
    public bool Complete { get; init; }
    public string? FilledByName { get; init; }
    public string? RawToken { get; init; }
    public bool Submitted { get; init; }

    /// <summary>
    /// True when the form is at the stage where this section's link works
    /// (AwaitingImamSignoff for the ceremony witnesses, AwaitingWitnesses otherwise).
    /// </summary>
    public bool Fillable { get; init; }
}