using Infrastructure.DTOs.Certificates;

namespace Application.Interfaces;

/// <summary>
/// Wedding certificate catalogue (list of issued nikah certificates).
/// </summary>
public interface ICertificateService
{
    Task<List<CertificateDto>> GetAllCertificatesAsync();
}