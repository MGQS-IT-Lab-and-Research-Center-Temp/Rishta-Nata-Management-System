using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BrideGuardianService : IBrideGuardianService
{
    private readonly RishtanataDbContext _context;

    public BrideGuardianService(RishtanataDbContext context)
    {
        _context = context;
    }

    // Creates new bride guardian record in the database and returns the created entity.
    public async Task<BrideGuardian?> CreateAsync(
        BrideGuardian guardian,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(guardian);

        _context.Set<BrideGuardian>().Add(guardian);
        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Set<BrideGuardian>()
            .FirstOrDefaultAsync(x => x.BrideGuardianId == guardian.BrideGuardianId, cancellationToken);
    }

    // Retrieves a bride guardian record by its unique identifier (ID) from the database.
    public async Task<BrideGuardian?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<BrideGuardian>()
            .FirstOrDefaultAsync(x => x.BrideGuardianId == id, cancellationToken);
    }

    // Retrieves a bride guardian record associated with a specific bride's ID from the database.
    public async Task<BrideGuardian?> GetByBrideIdAsync(Guid brideId, CancellationToken cancellationToken = default)
    {
        return await _context.JamaatMembers
            .Where(x => x.Id == brideId)
            .Select(x => x.BrideGuardian)
            .FirstOrDefaultAsync(cancellationToken);
    }

    // Assigns a bride guardian to a specific bride 
    // by updating the bride's record in the database with the guardian's ID.
    public async Task<bool> AssignToBrideAsync(Guid guardianId, Guid brideId, CancellationToken cancellationToken = default)
    {
        var bride = await _context.JamaatMembers
            .FirstOrDefaultAsync(x => x.Id == brideId, cancellationToken);


        if (bride is null)
        {
            return false;
        }

        var guardianExists = await _context.Set<BrideGuardian>()
            .AnyAsync(x => x.BrideGuardianId == guardianId, cancellationToken);

        if (!guardianExists)
        {
            return false;
        }

        bride.BrideGuardianId = guardianId;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    // Retrieve a bride guardian record associated with a specific marriage application ID from the database.
    public async Task<BrideGuardian?> GetByMarriageApplicationIdAsync(Guid marriageApplicationId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<BrideGuardian>()
            .FirstOrDefaultAsync(
                x => x.MarriageApplicationId == marriageApplicationId,
                cancellationToken);
    }

    // Updates an existing bride guardian record in the database 
    // and returns a boolean indicating whether the update was successful.
    // public async Task<bool> UpdateAsync(BrideGuardian guardian,CancellationToken cancellationToken = default)
    // {
    //     ArgumentNullException.ThrowIfNull(guardian);

    //     _context.BrideGuardians.Update(guardian);
    //     return await _context.SaveChangesAsync(cancellationToken) > 0;
    // }
}