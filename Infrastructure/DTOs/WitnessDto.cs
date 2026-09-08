namespace Infrastructure.DTOs;

public class WitnessDto
{
    public Guid MarriageApplicationId { get; set; }
    public string WitnessOneName { get; set; } = string.Empty;
    public string WitnessOneAddress { get; set; } = string.Empty;
    public string WitnessOneTel { get; set; } = string.Empty;
    public string WitnessOneSignatureDate { get; set; } = string.Empty;
    public string WitnessTwoName { get; set; } = string.Empty;
    public string WitnessTwoAddress { get; set; } = string.Empty;
    public string WitnessTwoTel { get; set; } = string.Empty;
    public string WitnessTwoSignatureDate { get; set; } = string.Empty;
}