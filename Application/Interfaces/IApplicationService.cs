using Application.DTOs.MarriageApplication;
﻿
using Application.DTOs.MarriageApplication;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto dto);
      
      Task<List<Domain.Entities.Application>> GetAllApplicationsAsync();  
      
      Task<ApplicationDto> GetApplicationByIdAsync(Guid id);

        Task<List<Domain.Entities.Application>> GetAllAsync();

        Task<List<Domain.Entities.Application>> GetApplicationsByJamaatAsync(Guid jamaatId);

        Task<bool> ApproveApplicationAsync(Guid applicationId);

        Task<bool> RejectApplicationAsync(Guid applicationId);
        Task<bool> RequestMoreInformationAsync(Guid id);
    }
}