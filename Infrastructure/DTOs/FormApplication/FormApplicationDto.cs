using Domain.Enums;
using Infrastructure.DTOs.Certificates;

<<<<<<< HEAD
namespace Infrastructure.DTOs.FormApplication;
=======
namespace Infrastructure.DTOs.MarriageApplication;
>>>>>>> origin/Dev

public class FormApplicationDto
{
    public Guid Id { get; set; }
    public ApplicationStatus Status { get; set; }
    public Guid UserId { get; set; }
    public string? SerialNumber { get; set; }

    public CertificateDto? Certificate { get; set; }
}