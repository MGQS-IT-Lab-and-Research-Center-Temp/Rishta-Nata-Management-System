using Domain.Entities;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;

namespace Infrastructure.Mapper;

public static class PendingApprovalMapper
{
    public static PendingApprovalDto ToDto(MarriageApplicationForm entity)
    {
        return new PendingApprovalDto
        {
            Id = entity.MarriageApplicationId,
            ApplicationNumber = entity.ReferenceNumber,
            GroomName = entity.BridegroomName,
            BrideName = entity.BrideName,
            PresidentName = entity.JamaatPresidentName,
            SubmittedDate = entity.CreatedAt,
            Status = entity.MarriageApplication.Status.ToString()
        };
    }
}