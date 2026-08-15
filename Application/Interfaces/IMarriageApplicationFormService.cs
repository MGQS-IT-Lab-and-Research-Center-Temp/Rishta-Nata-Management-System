using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMarriageApplicationFormService
    {
        /// <summary>
        /// Creates a new marriage application.
        /// </summary>
        Task<MarriageApplicationForm> CreateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a marriage application by its Id.
        /// </summary>
        Task<MarriageApplicationForm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing marriage application.
        /// Returns true if any rows were affected.
        /// </summary>
        Task<bool> UpdateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default);
    }
}