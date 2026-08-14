
using Application.DTOs.MarriageApplication;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMarriageApplicationService
    {
        Task<MarriageApplicationDto> CreateApplicationAsync(CreateMarriageApplicationDto dto);
        Task<MarriageApplicationDto> GetApplicationByIdAsync(Guid id);
        Task<List<MarriageApplication>> GetAllAsync();
    }
}
