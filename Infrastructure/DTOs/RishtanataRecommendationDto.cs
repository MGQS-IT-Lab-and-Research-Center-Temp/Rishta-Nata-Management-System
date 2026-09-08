namespace Infrastructure.DTOs;

public class RishtanataRecommendationDto
{
    public Guid MarriageApplicationId { get; set; }
    public string NationalRishtanataSecretaryName { get; set; } = string.Empty;
    public string NationalRishtanataSecretarySignatureDate { get; set; } = string.Empty;
}