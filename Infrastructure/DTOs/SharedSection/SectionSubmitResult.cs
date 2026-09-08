namespace Infrastructure.DTOs.SharedSection;

public class SectionSubmitResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public bool StageAdvanced { get; init; }
}