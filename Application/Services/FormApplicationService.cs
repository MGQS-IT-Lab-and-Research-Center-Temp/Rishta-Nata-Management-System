using Application.DTOs.MarriageApplication;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
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

        public async Task<List<FormApplication>> GetAllApplicationsAsync()
        {
            return new List<FormApplication>();
        }

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

        // why returning a new form application dto??
        public async Task<FormApplicationDto> GetApplicationByIdAsync(Guid id)
        {
            var marriageApplication = await _context.FormApplications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (marriageApplication == null)
            {
                throw new KeyNotFoundException(
                    $"Marriage application with ID {id} was not found.");
            }

            return new FormApplicationDto
            {
                Id = marriageApplication.Id,
                Status = marriageApplication.Status,
            };
        }

        public async Task<List<FormApplication>> GetAllAsync()
        {
            return await _context.FormApplications
                .ToListAsync();
        }
        public async Task<List<FormApplication>>
        GetPendingApplicationsAsync()
        {
            return await _context.FormApplications
                .Where(x =>
                    x.Status == ApplicationStatus.ApplicationPending)
                .Include(x => x.MarriageApplicationForm)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<bool> ApproveApplicationAsync(Guid id)
        {
            var application = await _context.FormApplications
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
            var application = await _context.FormApplications
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

            //later, modifiedat will always be updated in a savechangesasync that will be configured in dbcontext
            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RequestMoreInformationAsync(Guid id)
        {
            var application = await _context.FormApplications
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

        public async Task<List<FormApplication>> GetApplicationsByJamaatAsync(Guid jamaatId)
        {
            // TODO: Implement filtering by jamaatId when jamaatId is added to the Application entity
            // For now, return all applications
            return await _context.FormApplications
                .Include(x => x.MarriageApplicationForm)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
