using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.DTOs.Bride;


namespace Application.Interfaces.Service
{
    public interface IBrideFormSectionService
    {
        Task<BrideDto> CreateAsync(CreateBrideFormSectionDto dto);

        Task<BrideDto?> GetByIdAsync(Guid id);

        Task<BrideDto?> GetByMarriageApplicationFormIdAsync(Guid marriageApplicationFormId);

        Task UpdateAsync(Guid id, UpdateBrideFormSectionDto dto);

        Task DeleteAsync(Guid id);
    }
}
