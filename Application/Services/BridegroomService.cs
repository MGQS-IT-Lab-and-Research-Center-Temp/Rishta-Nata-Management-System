using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BridegroomService : IBridegroomService
{
    private readonly RishtanataDbContext _dbContext;

    public BridegroomService(RishtanataDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BrideGroom> CreateAsync(
        BrideGroom bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        _dbContext.BrideGrooms.Add(bridegroom);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return bridegroom;
    }

    public async Task<BrideGroom?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.BrideGrooms
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<BrideGroom?> GetByMembershipNoAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.BrideGrooms
            .FirstOrDefaultAsync(
                x => x.BridegroomMembershipNo == membershipNo,
                cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        BrideGroom bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        _dbContext.BrideGrooms.Update(bridegroom);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);

        return affected > 0;
    }
}
