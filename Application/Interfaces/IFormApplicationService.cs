using Domain.Entities;
using Infrastructure.DTOs.FormApplication;


namespace Application.Interfaces
{
    /// <summary>
    /// CRUD over FormApplication (the application wrapper).
    /// </summary>
    public interface IFormApplicationService
    {
        Task<FormApplicationDto> CreateApplicationAsync(CreateFormApplicationDto dto);
        Task<FormApplicationDto> GetApplicationByIdAsync(Guid id);
        Task<List<FormApplication>> GetAllApplicationsAsync();
        Task<List<FormApplication>> GetPendingApplicationsAsync();
        Task<bool> ApproveApplicationAsync(Guid id);
        Task<bool> RejectApplicationAsync(Guid id);
        Task<bool> RequestMoreInformationAsync(Guid id);
        Task<List<FormApplication>> GetApplicationsByJamaatAsync(Guid jamaatId);
    }
}
