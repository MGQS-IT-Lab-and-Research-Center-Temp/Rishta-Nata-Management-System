using Domain.Enums;

namespace Infrastructure.DTOs.SharedSection;

public class SectionLinkStatus
{
    public SectionType Section { get; init; }
    public bool HasActiveToken { get; init; }
    public bool Complete { get; init; }
    public string? FilledByName { get; init; }
}