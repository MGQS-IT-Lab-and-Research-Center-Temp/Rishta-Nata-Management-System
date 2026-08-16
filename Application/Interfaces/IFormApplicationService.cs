using Domain.Entities;
using Infrastructure.DTOs.MarriageApplication;

namespace Application.Interfaces
{
    public interface IFormApplicationService
    {
        Task<FormApplicationDto> CreateApplicationAsync(CreateFormApplicationDto dto);

        Task<List<FormApplication>> GetAllApplicationsAsync();

        Task<FormApplicationDto> GetApplicationByIdAsync(Guid id);

        Task<List<FormApplication>> GetAllAsync();

        Task<List<FormApplication>> GetApplicationsByJamaatAsync(Guid jamaatId);

        Task<bool> ApproveApplicationAsync(Guid applicationId);

        Task<bool> RejectApplicationAsync(Guid applicationId);
        Task<bool> RequestMoreInformationAsync(Guid id);
    }
}