using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

namespace Presentation.Mapping.RishtanataSecretary;

public static class PendingApprovalMapping
{
    public static PendingApprovalViewModel ToViewModel(PendingApprovalDto dto)
    {
        return new PendingApprovalViewModel
        {
            Id = dto.Id,
            ApplicationNumber = dto.ApplicationNumber,
            GroomName = dto.GroomName,
            BrideName = dto.BrideName,
            JamaatName = dto.JamaatName,
            PresidentName = dto.PresidentName,
            SubmittedDate = dto.SubmittedDate,
            PresidentRecommendation = dto.PresidentRecommendation,
            Status = dto.Status
        };
    }
}