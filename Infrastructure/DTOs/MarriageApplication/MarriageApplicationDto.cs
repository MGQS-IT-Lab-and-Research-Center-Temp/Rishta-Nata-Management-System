using Domain.Enums;

namespace Application.DTOs.MarriageApplication;

public class MarriageApplicationDto
{
    public Guid Id { get; set; }
    public ApplicationStatus Status { get; set; }
    public Guid UserId { get; set; }
    public string? SerialNumber { get; set; }

    public CertificateDto? Certificate { get; set; }
}