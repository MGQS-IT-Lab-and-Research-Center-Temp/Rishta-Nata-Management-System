using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMarriageApplicationFormService
    {
        //Task<MarriageApplicationForm> CreateAsync(MarriageApplicationForm application);
        Task<MarriageApplicationForm> CreateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default);

        // Task<MarriageApplicationForm?> GetByIdAsync(Guid id);
        Task<MarriageApplicationForm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

       
        Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(Guid marriageAplicationId);


        /// <summary>
        /// Updates an existing marriage application.
        /// Returns true if any rows were affected.
        /// </summary>


        //Task<bool> UpdateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(MarriageApplicationForm application, CancellationToken cancellationToken = default);

    }
    
}
