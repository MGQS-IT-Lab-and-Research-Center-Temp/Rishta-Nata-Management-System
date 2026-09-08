namespace Infrastructure.DTOs;

public class GuardianOrWakeelDto
{
    public Guid MarriageApplicationId { get; set; }
    public string GuardianName { get; set; } = string.Empty;
    public string GuardianRelationToBride { get; set; } = string.Empty;
    public string GuardianAddress { get; set; } = string.Empty;
    public string GuardianTel { get; set; } = string.Empty;
    public string GuardianSignatureDate { get; set; } = string.Empty;
    public string RepresentativeName { get; set; } = string.Empty;
    public string RepresentativeAddress { get; set; } = string.Empty;
    public string RepresentativeActingFor { get; set; } = string.Empty;
    public string RepresentativeSignatureDate { get; set; } = string.Empty;
}