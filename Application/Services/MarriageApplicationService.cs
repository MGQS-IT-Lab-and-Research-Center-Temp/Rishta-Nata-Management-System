using Application.DTOs.MarriageApplication;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class MarriageApplicationService : IMarriageApplicationService
    {
        private readonly RishtanataDbContext _context;

        public MarriageApplicationService(RishtanataDbContext context)
        {
            _context = context;
        }

        public async Task<MarriageApplicationDto> CreateApplicationAsync(
            CreateMarriageApplicationDto dto)
        {
            var marriageApplication = new MarriageApplication
            {
                UserId = dto.UserId
            };

            _context.MarriageApplications.Add(marriageApplication);

            await _context.SaveChangesAsync();

            return new MarriageApplicationDto
            {
                Id = marriageApplication.Id,
                Status = marriageApplication.Status,
                UserId = marriageApplication.UserId,
                SerialNumber = marriageApplication.SerialNumber
            };
        }

        public async Task<MarriageApplicationDto> GetApplicationByIdAsync(Guid id)
        {
            var marriageApplication = await _context.MarriageApplications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (marriageApplication == null)
            {
                throw new KeyNotFoundException(
                    $"Marriage application with ID {id} was not found.");
            }

            return new MarriageApplicationDto
            {
                Id = marriageApplication.Id,
                Status = marriageApplication.Status,
                UserId = marriageApplication.UserId,
                SerialNumber = marriageApplication.SerialNumber
            };
        }

        public async Task<List<MarriageApplication>> GetAllAsync()
        {
            return await _context.MarriageApplications
                .ToListAsync();
        }
    }
}
