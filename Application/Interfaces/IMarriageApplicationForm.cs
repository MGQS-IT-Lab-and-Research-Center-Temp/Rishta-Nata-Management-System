using System;
using System.Collections.Generic;
using System.Text;


    using Domain.Entities;

    namespace Application.Interfaces;

    public interface IMarriageApplicationFormService
    {
        Task<MarriageApplicationForm> CreateAsync(
            MarriageApplicationForm application);

        Task<MarriageApplicationForm?> GetByIdAsync(Guid id);

        Task<bool> UpdateAsyn(MarriageApplicationForm application);
    }

