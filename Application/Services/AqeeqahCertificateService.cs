using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DTOs.Certificates;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AqeeqahCertificateService : IAqeeqahCertificateService
    {
        private readonly RishtanataDbContext _context;

        public AqeeqahCertificateService(RishtanataDbContext context)
        {
            _context = context;
        }

        public async Task<List<AqeeqahCertificateDto>> GetAllCertificatesAsync()
        {
            var certificates = await _context.AqeeqahCertificates
                .OrderByDescending(x => x.IssueDate)
                .ToListAsync();

            return certificates.Select(MapToDto).ToList();
        }

        public async Task<List<AqeeqahCertificateDto>> GetCertificatesByJamaatAsync(Guid jamaatId)
        {
            var certificates = await _context.AqeeqahCertificates
                .Where(x => x.JamaatId == jamaatId)
                .OrderByDescending(x => x.IssueDate)
                .ToListAsync();

            return certificates.Select(MapToDto).ToList();
        }

        public async Task<AqeeqahCertificateDto?> GetCertificateByIdAsync(Guid id)
        {
            var certificate = await _context.AqeeqahCertificates
                .FirstOrDefaultAsync(x => x.Id == id);

            return certificate != null ? MapToDto(certificate) : null;
        }

        public async Task<AqeeqahCertificateDto> CreateCertificateAsync(AqeeqahCertificateDto dto)
        {
            var certificate = new AqeeqahCertificate
            {
                SerialNumber = dto.SerialNumber,
                ChildName = dto.ChildName,
                FatherName = dto.FatherName,
                MotherName = dto.MotherName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                AqeeqahDate = dto.AqeeqahDate,
                AqeeqahLocation = dto.AqeeqahLocation,
                AnimalCount = dto.AnimalCount,
                IssueDate = dto.IssueDate,
                CertificateFilePath = dto.CertificateFilePath
            };

            _context.AqeeqahCertificates.Add(certificate);
            await _context.SaveChangesAsync();

            return MapToDto(certificate);
        }

        public async Task<bool> UpdateCertificateAsync(Guid id, AqeeqahCertificateDto dto)
        {
            var certificate = await _context.AqeeqahCertificates
                .FirstOrDefaultAsync(x => x.Id == id);

            if (certificate == null)
                return false;

            certificate.SerialNumber = dto.SerialNumber;
            certificate.ChildName = dto.ChildName;
            certificate.FatherName = dto.FatherName;
            certificate.MotherName = dto.MotherName;
            certificate.DateOfBirth = dto.DateOfBirth;
            certificate.Gender = dto.Gender;
            certificate.AqeeqahDate = dto.AqeeqahDate;
            certificate.AqeeqahLocation = dto.AqeeqahLocation;
            certificate.AnimalCount = dto.AnimalCount;
            certificate.IssueDate = dto.IssueDate;
            certificate.CertificateFilePath = dto.CertificateFilePath;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCertificateAsync(Guid id)
        {
            var certificate = await _context.AqeeqahCertificates
                .FirstOrDefaultAsync(x => x.Id == id);

            if (certificate == null)
                return false;

            _context.AqeeqahCertificates.Remove(certificate);
            await _context.SaveChangesAsync();
            return true;
        }

        private static AqeeqahCertificateDto MapToDto(AqeeqahCertificate certificate)
        {
            return new AqeeqahCertificateDto
            {
                Id = certificate.Id,
                SerialNumber = certificate.SerialNumber,
                ChildName = certificate.ChildName,
                FatherName = certificate.FatherName,
                MotherName = certificate.MotherName,
                DateOfBirth = certificate.DateOfBirth,
                Gender = certificate.Gender,
                AqeeqahDate = certificate.AqeeqahDate,
                AqeeqahLocation = certificate.AqeeqahLocation,
                AnimalCount = certificate.AnimalCount,
                IssueDate = certificate.IssueDate,
                CertificateFilePath = certificate.CertificateFilePath
            };
        }
    }
}
