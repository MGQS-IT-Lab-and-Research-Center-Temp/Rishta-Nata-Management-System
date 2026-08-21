using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.DTOs.Bride;


namespace Application.Interfaces.Service
{
    public interface IBrideService
    {
        Task<BrideDto> CreateAsync(CreateBrideDto dto);

        Task<BrideDto?> GetByIdAsync(Guid id);

        Task<BrideDto?> GetByMarriageApplicationFormIdAsync(Guid marriageApplicationFormId);

        Task UpdateAsync(Guid id, UpdateBrideDto dto);

        Task DeleteAsync(Guid id);
    }
}
