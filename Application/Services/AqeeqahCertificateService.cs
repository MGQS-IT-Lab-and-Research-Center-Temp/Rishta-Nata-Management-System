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

        // =========================================================
        // GET ALL
        // =========================================================
        public async Task<List<AqeeqahCertificateDto>> GetAllCertificatesAsync()
        {
            var certificates = await _context.AqeeqahCertificates
                .AsNoTracking()
                .OrderByDescending(x => x.IssueDate)
                .ToListAsync();

            return certificates
                .Select(MapToDto)
                .ToList();
        }

        // =========================================================
        // GET BY JAMAAT
        // =========================================================
        public async Task<List<AqeeqahCertificateDto>> GetCertificatesByJamaatAsync(
            Guid jamaatId)
        {
            var certificates = await _context.AqeeqahCertificates
                .AsNoTracking()
                .Where(x => x.JamaatId == jamaatId)
                .OrderByDescending(x => x.IssueDate)
                .ToListAsync();

            return certificates
                .Select(MapToDto)
                .ToList();
        }

        // =========================================================
        // GET BY ID
        // =========================================================
        public async Task<AqeeqahCertificateDto?> GetCertificateByIdAsync(
            Guid id)
        {
            var certificate = await _context.AqeeqahCertificates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return certificate == null
                ? null
                : MapToDto(certificate);
        }

        // =========================================================
        // CREATE
        // =========================================================
        public async Task<AqeeqahCertificateDto> CreateCertificateAsync(
            AqeeqahCertificateDto dto)
        {
            var certificate = new AqeeqahCertificate
            {
                SerialNumber = dto.SerialNumber,

                // Child
                ChildName = dto.ChildName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PlaceOfBirth = dto.PlaceOfBirth,

                // Parents
                FatherName = dto.FatherName,
                MotherName = dto.MotherName,

                // Jamaat
                JamaatId = dto.JamaatId,

                // Address
                Address = dto.Address,

                // Missionary
                OfficiatingMissionary = dto.OfficiatingMissionary,

                // Aqeeqah
                AqeeqahDate = dto.AqeeqahDate,
                AqeeqahLocation = dto.AqeeqahLocation,
                AnimalCount = dto.AnimalCount,

                // Administration
                IssueDate = dto.IssueDate,
                IssuedByUserId = dto.IssuedByUserId,

                // Generated file
                CertificateFilePath = dto.CertificateFilePath
            };

            _context.AqeeqahCertificates.Add(certificate);

            await _context.SaveChangesAsync();

            return MapToDto(certificate);
        }

        // =========================================================
        // UPDATE
        // =========================================================
        public async Task<bool> UpdateCertificateAsync(
            Guid id,
            AqeeqahCertificateDto dto)
        {
            var certificate = await _context.AqeeqahCertificates
                .FirstOrDefaultAsync(x => x.Id == id);

            if (certificate == null)
                return false;

            // Certificate
            certificate.SerialNumber = dto.SerialNumber;

            // Child
            certificate.ChildName = dto.ChildName;
            certificate.DateOfBirth = dto.DateOfBirth;
            certificate.Gender = dto.Gender;
            certificate.PlaceOfBirth = dto.PlaceOfBirth;

            // Parents
            certificate.FatherName = dto.FatherName;
            certificate.MotherName = dto.MotherName;

            // Jamaat
            certificate.JamaatId = dto.JamaatId;

            // Address
            certificate.Address = dto.Address;

            // Missionary
            certificate.OfficiatingMissionary =
                dto.OfficiatingMissionary;

            // Aqeeqah
            certificate.AqeeqahDate = dto.AqeeqahDate;
            certificate.AqeeqahLocation = dto.AqeeqahLocation;
            certificate.AnimalCount = dto.AnimalCount;

            // Administration
            certificate.IssueDate = dto.IssueDate;
            certificate.IssuedByUserId = dto.IssuedByUserId;

            // Generated file
            certificate.CertificateFilePath =
                dto.CertificateFilePath;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================================
        // DELETE
        // =========================================================
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

        // =========================================================
        // ENTITY → DTO
        // =========================================================

        private static AqeeqahCertificateDto MapToDto(
    AqeeqahCertificate certificate)
        {
            return new AqeeqahCertificateDto
            {
                Id = certificate.Id,

                SerialNumber = certificate.SerialNumber,

                ChildName = certificate.ChildName,
                DateOfBirth = certificate.DateOfBirth,
                Gender = certificate.Gender,
                PlaceOfBirth = certificate.PlaceOfBirth,

                JamaatId = certificate.JamaatId,
                JamaatName = certificate.JamaatName,

                FatherName = certificate.FatherName,
                MotherName = certificate.MotherName,

                Address = certificate.Address,

                OfficiatingMissionary =
                    certificate.OfficiatingMissionary,

                IssueDate = certificate.IssueDate,
                IssuedByUserId = certificate.IssuedByUserId,

                AqeeqahDate = certificate.AqeeqahDate,
                AqeeqahLocation = certificate.AqeeqahLocation,
                AnimalCount = certificate.AnimalCount,

                CertificateFilePath =
                    certificate.CertificateFilePath

            };
        }
    }
}