using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels;

namespace Presentation.Mapping.RishtanataSecretary;

public static class RecentActivityMapping
{
    public static RecentActivityViewModel ToViewModel(RecentActivityDto dto)
    {
        return new RecentActivityViewModel
        {
            ApplicationNumber = dto.ActivityType,
            Description = dto.Description,
            Date = dto.ActivityDate
        };
    }
}