using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MarriageApplicationFormService : IMarriageApplicationFormService
{
    private readonly RishtanataDbContext _context;

    public MarriageApplicationFormService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<MarriageApplicationForm> CreateAsync(
        MarriageApplicationForm application)
    {
        _context.MarriageApplicationForms.Add(application);

        await _context.SaveChangesAsync();

        return application;
    }

    public async Task<MarriageApplicationForm?> GetByIdAsync(Guid id)
    {
        return await _context.MarriageApplicationForms
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateAsyn(MarriageApplicationForm application)
    {
        _context.MarriageApplicationForms.Update(application);

        await _context.SaveChangesAsync();

        return true;
    }
}