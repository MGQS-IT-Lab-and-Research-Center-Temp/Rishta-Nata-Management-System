namespace Infrastructure.DTOs;

public class JamaatPresidentVerificationDto
{
    public Guid MarriageApplicationId { get; set; }
    public string JamaatPresidentName { get; set; } = string.Empty;
    public string JamaatPresidentSignatureDate { get; set; } = string.Empty;
}