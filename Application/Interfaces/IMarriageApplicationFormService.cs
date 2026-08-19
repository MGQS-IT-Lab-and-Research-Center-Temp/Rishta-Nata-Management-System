using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMarriageApplicationFormService
    {
        Task<MarriageApplicationForm?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<MarriageApplicationForm?> GetByMembershipNoAsync(string membershipNo, CancellationToken ct = default);
        Task CreateAsync(MarriageApplicationForm form, CancellationToken ct = default);
        Task<MarriageApplicationForm> CreateDraftForGroomAsync(MarriageApplicationForm form, CancellationToken ct = default);
        Task UpdateAsync(MarriageApplicationForm form, CancellationToken ct = default);
    }
}
