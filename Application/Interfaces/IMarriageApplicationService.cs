
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMarriageApplicationService
    {
        Task<MarriageApplication> CreateAsync(MarriageApplication marriageApplication);
        Task<MarriageApplication?> GetByIdAsync(Guid id);
        Task<List<MarriageApplication>> GetAllAsync();
    }
}
