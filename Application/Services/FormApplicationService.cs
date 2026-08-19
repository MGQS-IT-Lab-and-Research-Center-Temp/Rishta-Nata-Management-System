using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.FormApplication;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class FormApplicationService : IFormApplicationService
    {
        private readonly RishtanataDbContext _context;

        public FormApplicationService(RishtanataDbContext context)
        {
            _context = context;
        }

        // ✅ Create a new application
        public async Task<FormApplicationDto> CreateApplicationAsync(CreateFormApplicationDto dto)
        {
            var application = new FormApplication
            {
                Status = dto.Status,
                MarriageApplicationFormId = dto.MarriageApplicationFormId,
                CertificateId = dto.CertificateId,
                AppliedAt = dto.AppliedAt
            };

            _context.FormApplications.Add(application);
            await _context.SaveChangesAsync();

            return new FormApplicationDto
            {
                Id = application.Id,
                Status = application.Status
            };
        }

        // ✅ Get a single application by ID
        public async Task<FormApplicationDto> GetApplicationByIdAsync(Guid id)
        {
            var application = await _context.FormApplications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (application == null)
                throw new KeyNotFoundException($"Application with ID {id} was not found.");

            return new FormApplicationDto
            {
                Id = application.Id,
                Status = application.Status
            };
        }

        // ✅ Get all applications
        public async Task<List<FormApplication>> GetAllApplicationsAsync()
        {
            return await _context.FormApplications.ToListAsync();
        }

        // ✅ Get pending applications
        public async Task<List<FormApplication>> GetPendingApplicationsAsync()
        {
            return await _context.FormApplications
                .Where(x => x.Status == ApplicationStatus.ApplicationPending)
                .Include(x => x.MarriageApplicationForm)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // ✅ Approve application
        public async Task<bool> ApproveApplicationAsync(Guid id)
        {
            var application = await _context.FormApplications.FirstOrDefaultAsync(x => x.Id == id);

            if (application == null || application.Status != ApplicationStatus.ApplicationPending)
                return false;

            application.Status = ApplicationStatus.ApplicationApproved;
            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Reject application
        public async Task<bool> RejectApplicationAsync(Guid id)
        {
            var application = await _context.FormApplications.FirstOrDefaultAsync(x => x.Id == id);

            if (application == null || application.Status != ApplicationStatus.ApplicationPending)
                return false;

            application.Status = ApplicationStatus.ApplicationRejected;
            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Request more information (optional new status)
        public async Task<bool> RequestMoreInformationAsync(Guid id)
        {
            var application = await _context.FormApplications.FirstOrDefaultAsync(x => x.Id == id);

            if (application == null || application.Status != ApplicationStatus.ApplicationPending)
                return false;

            // If you have a distinct enum value for "NeedsMoreInfo", use that instead
            application.Status = ApplicationStatus.ApplicationPending;
            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Get applications by Jamaat (placeholder until filtering is implemented)
        public async Task<List<FormApplication>> GetApplicationsByJamaatAsync(Guid jamaatId)
        {
            return await _context.FormApplications
                .Include(x => x.MarriageApplicationForm)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
