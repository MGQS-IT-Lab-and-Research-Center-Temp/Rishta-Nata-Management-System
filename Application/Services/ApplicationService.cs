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
            //    var application = new Application
            //    {
            //     Status = dto.Status,

            //public Guid MarriageApplicationFormId { get; set; }
            //public MarriageApplicationForm MarriageApplicationForm { get; set; } = default!;
            ////public Guid UserId { get; set; }
            ////public User User { get; set; }
            //public Guid CertificateId { get; set; }
            //public Certificate Certificate { get; set; } = default!;
            //public DateTime AppliedAt { get; set; }
            //    };

            //_context.Applications.Add(application);

            //await _context.SaveChangesAsync();

            //return new ApplicationDto
            //{
            //    Id = marriageApplication.Id,
            //    Status = marriageApplication.Status,
            //    UserId = marriageApplication.UserId,
            //    SerialNumber = marriageApplication.SerialNumber
            //};
            return null!;
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
    }
}
