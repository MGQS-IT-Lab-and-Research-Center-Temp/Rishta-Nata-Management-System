using Infrastructure.DTOs.Certificates;

namespace Application.Interfaces
{
    public interface IAqeeqahCertificateService
    {
        /// <summary>
        /// Gets all Aqeeqah certificates.
        /// </summary>
        Task<List<AqeeqahCertificateDto>> GetAllCertificatesAsync();

        /// <summary>
        /// Gets Aqeeqah certificates for a specific Jamaat.
        /// </summary>
        Task<List<AqeeqahCertificateDto>> GetCertificatesByJamaatAsync(Guid jamaatId);

        /// <summary>
        /// Gets a specific Aqeeqah certificate by ID.
        /// </summary>
        Task<AqeeqahCertificateDto?> GetCertificateByIdAsync(Guid id);

        /// <summary>
        /// Creates a new Aqeeqah certificate.
        /// </summary>
        Task<AqeeqahCertificateDto> CreateCertificateAsync(AqeeqahCertificateDto dto);

        /// <summary>
        /// Updates an existing Aqeeqah certificate.
        /// </summary>
        Task<bool> UpdateCertificateAsync(Guid id, AqeeqahCertificateDto dto);

        /// <summary>
        /// Deletes an Aqeeqah certificate.
        /// </summary>
        Task<bool> DeleteCertificateAsync(Guid id);
    }
}
