using Domain.Entities;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;

namespace Infrastructure.Mapper;

public static class RecentActivityMapper
{
    public static RecentActivityDto ToDto(AuditLog entity)
    {
        return new RecentActivityDto
        {
            Id = entity.Id,
            ActivityType = entity.EntityName,
            Description = entity.Action,
            ActivityDate = entity.Timestamp
        };
    }
}




