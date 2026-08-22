using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBridegroomService
    {
        Task<BrideGroom> CreateOrUpdateAsync(BrideGroom bridegroom, CancellationToken cancellationToken = default);
        Task<BrideGroom> CreateAsync(BrideGroom bridegroom, CancellationToken cancellationToken = default);
        Task<BrideGroom?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BrideGroom?> GetByMembershipNoAsync(string membershipNo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(BrideGroom bridegroom, CancellationToken cancellationToken = default);
    }
}
