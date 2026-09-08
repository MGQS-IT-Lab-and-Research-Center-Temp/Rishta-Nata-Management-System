using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.FormApplication;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Application.Services
{
    /// <summary>
    /// CRUD over FormApplication (the application wrapper holding form +
    /// certificate + status).
    /// </summary>
    public class FormApplicationService : IFormApplicationService
    {
        private readonly RishtanataDbContext _context;

        public FormApplicationService(RishtanataDbContext context)
        {
            _context = context;
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

        public async Task<List<FormApplication>> GetAllApplicationsAsync()
        {
            return await _context.FormApplications
                .ToListAsync();
        }
        public async Task<List<FormApplication>>
        GetPendingApplicationsAsync()
        {
            return await _context.FormApplications
                // Cleanup: AwaitingMoreInformation is also pending-ish — a form
                // sent back for corrections still awaits the next review step.
                .Where(x =>
                    x.Status == ApplicationStatus.ApplicationPending ||
                    x.Status == ApplicationStatus.AwaitingMoreInformation)
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

            // Cleanup: allow from AwaitingMoreInformation too, so a form that
            // was sent back for corrections isn't orphaned outside the flow.
            if (application.Status !=
                ApplicationStatus.ApplicationPending &&
                application.Status !=
                ApplicationStatus.AwaitingMoreInformation)
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

            // Cleanup: same orphan-avoidance rule as ApproveApplicationAsync —
            // reject is allowed from both pending-ish states.
            if (application.Status !=
                ApplicationStatus.ApplicationPending &&
                application.Status !=
                ApplicationStatus.AwaitingMoreInformation)
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

            // Cleanup: this was a no-op — it set a pending form back to pending.
            // It now moves the form to AwaitingMoreInformation so the state is
            // distinguishable from a form that was never reviewed.
            if (application.Status !=
                ApplicationStatus.ApplicationPending &&
                application.Status !=
                ApplicationStatus.AwaitingMoreInformation)
            {
                return false;
            }

            application.Status =
                ApplicationStatus.AwaitingMoreInformation;

            application.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<FormApplication>> GetApplicationsByJamaatAsync(Guid jamaatId)
        {
            // TODO: Implement filtering by jamaatId when jamaatId is added to the User entity
            // For now, return all applications
            return await _context.FormApplications
                .Include(x => x.MarriageApplicationForm)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}