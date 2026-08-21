using Domain.Enums;

namespace Infrastructure.DTOs;

public class WitnessDto
{
    public Guid Id { get; set; }

    public Guid MarriageApplicationFormId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;

    public WitnessRole Role { get; set; }

    public int WitnessNumber { get; set; }

    public string InvitationToken { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }
}