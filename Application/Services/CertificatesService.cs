using Application.Interfaces;
using Infrastructure.DTOs.Certificates;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CertificateService : ICertificateService
{
    private readonly RishtanataDbContext _context;

    public CertificateService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<List<CertificateDto>> GetAllCertificatesAsync()
    {
        return await _context.Certificates
            .AsNoTracking()
            .Select(c => new CertificateDto
            {
                Id = c.Id,
                SerialNumber = c.SerialNumber,
                BrideName = c.BrideName,
                BridegroomName = c.BridegroomName,
                NikahDate = c.NikahDate,
                IssueDate = c.IssueDate,
                CertificateFilePath = c.CertificateFilePath
            })
            .OrderByDescending(c => c.IssueDate)
            .ToListAsync();
    }
}