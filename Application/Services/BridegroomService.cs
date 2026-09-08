using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// BridegroomFormSection record management only. The staged bridegroom-section
/// submission moved to BridegroomSectionService (cleanup) so this class has a
/// single responsibility.
/// </summary>
public class BridegroomService : IBridegroomService
{
    private readonly RishtanataDbContext _dbContext;

    public BridegroomService(RishtanataDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BridegroomFormSection> CreateOrUpdateAsync(
        BridegroomFormSection bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        var existingBridegroom = await GetByMembershipNoAsync(
            bridegroom.BridegroomMembershipNo,
            cancellationToken);

        if (existingBridegroom is null)
        {
            return await CreateAsync(bridegroom, cancellationToken);
        }

        existingBridegroom.BridegroomName = bridegroom.BridegroomName;
        existingBridegroom.BridegroomDateOfBirth = bridegroom.BridegroomDateOfBirth;
        existingBridegroom.BridegroomResidentOf = bridegroom.BridegroomResidentOf;
        existingBridegroom.BridegroomGenotype = bridegroom.BridegroomGenotype;
        existingBridegroom.BridegroomBloodGroup = bridegroom.BridegroomBloodGroup;
        existingBridegroom.BridegroomDowerAmountPaidInCash = bridegroom.BridegroomDowerAmountPaidInCash;
        existingBridegroom.BridegroomDowerAmountToBePaid = bridegroom.BridegroomDowerAmountToBePaid;
        existingBridegroom.BridegroomSignatureTel = bridegroom.BridegroomSignatureTel;
        existingBridegroom.IsFirstNikah = bridegroom.IsFirstNikah;
        existingBridegroom.IsSecondThirdOrFourthNikah = bridegroom.IsSecondThirdOrFourthNikah;
        existingBridegroom.FormerWifeIsDead = bridegroom.FormerWifeIsDead;
        existingBridegroom.HasDivorcedFormerWife = bridegroom.HasDivorcedFormerWife;
        existingBridegroom.FormerWifeIsPresent = bridegroom.FormerWifeIsPresent;
        existingBridegroom.FormerWifeObtainedKhula = bridegroom.FormerWifeObtainedKhula;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return existingBridegroom;
    }

    public async Task<BridegroomFormSection> CreateAsync(
        BridegroomFormSection bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        _dbContext.BridegroomFormSections.Add(bridegroom);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return bridegroom;
    }

    public async Task<BridegroomFormSection?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.BridegroomFormSections
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<BridegroomFormSection?> GetByMembershipNoAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.BridegroomFormSections
            .FirstOrDefaultAsync(
                x => x.BridegroomMembershipNo == membershipNo,
                cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        BridegroomFormSection bridegroom,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bridegroom);

        _dbContext.BridegroomFormSections.Update(bridegroom);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);

        return affected > 0;
    }
}