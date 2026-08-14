using Infrastructure.Persistence;
using Application.Interfaces;
using Domain.Entities;
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

        public async Task<MarriageApplication> CreateAsync(
            MarriageApplication marriageApplication)
        {
            _context.MarriageApplications.Add(marriageApplication);

            await _context.SaveChangesAsync();

            return marriageApplication;
        }

        public async Task<MarriageApplication?> GetByIdAsync(Guid id)
        {
            return await _context.MarriageApplications
                .FindAsync(id);
        }

        public async Task<List<MarriageApplication>> GetAllAsync()
        {
            return await _context.MarriageApplications
                .ToListAsync();
        }
    }
}
