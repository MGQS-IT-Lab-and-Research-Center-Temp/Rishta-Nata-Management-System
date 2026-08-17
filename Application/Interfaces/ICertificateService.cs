using Infrastructure.DTOs.Certificates;

namespace Application.Interfaces;

public interface ICertificateService
{
    Task<List<CertificateDto>> GetAllCertificatesAsync();
}