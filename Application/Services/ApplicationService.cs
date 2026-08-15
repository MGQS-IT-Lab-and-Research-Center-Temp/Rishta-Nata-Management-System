using Application.DTOs.MarriageApplication;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly RishtanataDbContext _context;

        public ApplicationService(RishtanataDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto dto)
        {
            var application = new Domain.Entities.Application
            {
                Status = dto.Status,
                MarriageApplicationFormId = dto.MarriageApplicationFormId,
                CertificateId = dto.CertificateId,
                AppliedAt = dto.AppliedAt
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return new ApplicationDto
            {
                Id = application.Id,
                Status = application.Status
            };
        }

        public async Task<ApplicationDto> GetApplicationByIdAsync(Guid id)
        {
            var marriageApplication = await _context.Applications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (marriageApplication == null)
            {
                throw new KeyNotFoundException(
                    $"Marriage application with ID {id} was not found.");
            }

            return new ApplicationDto
            {
                Id = marriageApplication.Id,
                Status = marriageApplication.Status,
            };
        }

        public async Task<List<Domain.Entities.Application>> GetAllAsync()
        {
            return await _context.Applications
                .ToListAsync();
        }
        public async Task<List<Domain.Entities.Application>>
        GetPendingApplicationsAsync()
        {
            return await _context.Applications
                .Where(x =>
                    x.Status == ApplicationStatus.ApplicationPending)
                .Include(x => x.MarriageApplicationForm)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<bool> ApproveApplicationAsync(Guid id)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (application == null)
            {
                return false;
            }

            if (application.Status !=
                ApplicationStatus.ApplicationPending)
            {
                return false;
            }

            application.Status =
                ApplicationStatus.ApplicationApproved;

            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RejectApplicationAsync(Guid id)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (application == null)
            {
                return false;
            }

            if (application.Status !=
                ApplicationStatus.ApplicationPending)
            {
                return false;
            }

            application.Status =
                ApplicationStatus.ApplicationRejected;

            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RequestMoreInformationAsync(Guid id)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (application == null)
            {
                return false;
            }

            if (application.Status !=
                ApplicationStatus.ApplicationPending)
            {
                return false;
            }

            application.Status =
                ApplicationStatus.ApplicationPending;

            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Domain.Entities.Application>> GetApplicationsByJamaatAsync(Guid jamaatId)
        {
            // TODO: Implement filtering by jamaatId when jamaatId is added to the Application entity
            // For now, return all applications
            return await _context.Applications
                .Include(x => x.MarriageApplicationForm)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
