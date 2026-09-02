using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// BridegroomFormSection record management only. The staged bridegroom-section
    /// submission moved to IBridegroomSectionService (cleanup) so each interface
    /// has a single responsibility.
    /// </summary>
    public interface IBridegroomService
    {
        Task<BridegroomFormSection> CreateOrUpdateAsync(BridegroomFormSection bridegroom, CancellationToken cancellationToken = default);
        Task<BridegroomFormSection> CreateAsync(BridegroomFormSection bridegroom, CancellationToken cancellationToken = default);
        Task<BridegroomFormSection?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BridegroomFormSection?> GetByMembershipNoAsync(string membershipNo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(BridegroomFormSection bridegroom, CancellationToken cancellationToken = default);
    }
}