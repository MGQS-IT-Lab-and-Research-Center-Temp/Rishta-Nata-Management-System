using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Infrastructure.Persistence; 
public class MarriageApplicationFormService : IMarriageApplicationFormService
{
    private readonly RishtanataDbContext _dbContext;
    private readonly ILogger<MarriageApplicationFormService> _logger;

    public MarriageApplicationFormService(Infrastructure.Persistence.RishtanataDbContext dbContext, ILogger<MarriageApplicationFormService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<MarriageApplicationForm?> GetByMembershipNoAsync(string membershipNo, CancellationToken ct = default)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(f => f.BridegroomMembershipNo == membershipNo, ct);
    }

    public async Task<MarriageApplicationForm> CreateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default)
    {
        if (application is null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Add(application);

        var saved = await _dbContext.SaveChangesAsync(cancellationToken);
        return application;
    }

    public async Task<MarriageApplicationForm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(Guid marriageAplicationId)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(x => x.MarriageApplicationId == marriageAplicationId);
    }

    public async Task<bool> UpdateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default)
    {
        if (application is null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Update(application);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);
        return affected > 0;
    }
}
