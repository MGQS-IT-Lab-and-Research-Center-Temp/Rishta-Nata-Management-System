
using Infrastructure.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto dto);
        Task<ApplicationDto> GetApplicationByIdAsync(Guid id);
        Task<List<Domain.Entities.Application>> GetAllAsync();
    }
}
