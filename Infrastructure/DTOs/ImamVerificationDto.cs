namespace Infrastructure.DTOs;

public class ImamVerificationDto
{
    public Guid MarriageApplicationId { get; set; }
    public string OfficiatingImamName { get; set; } = string.Empty;
    public string OfficiatingImamAddressJamaat { get; set; } = string.Empty;
    public string OfficiatingImamSignatureDate { get; set; } = string.Empty;
}