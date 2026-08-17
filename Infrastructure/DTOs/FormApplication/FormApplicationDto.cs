using Domain.Enums;

namespace Infrastructure.DTOs.MarriageApplication;

public class FormApplicationDto
{
    public Guid Id { get; set; }
    public ApplicationStatus Status { get; set; }
    public Guid UserId { get; set; }
    public string? SerialNumber { get; set; }

    public CertificateDto? Certificate { get; set; }
}