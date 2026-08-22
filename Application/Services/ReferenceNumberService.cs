using Application.Interfaces.Service;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReferenceNumberService : IReferenceNumberService
{
    private readonly RishtanataDbContext _context;

    public ReferenceNumberService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAsync()
    {
        var year = DateTime.UtcNow.Year;

        var count = await _context.MarriageApplicationForms
            .CountAsync();

        var nextNumber = count + 1;

        return $"AMJN/{year}/{nextNumber:D6}";
    }
}

