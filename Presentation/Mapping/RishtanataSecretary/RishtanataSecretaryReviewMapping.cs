using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels;


namespace Presentation.Mapping.RishtanataSecretary;

    public static class RishtanataSecretaryReviewMapping
    {

    public static ReviewApplicationViewModel ToViewModel(ReviewApplicationDto dto)
    {
        return new ReviewApplicationViewModel
        {
            Id = dto.Id,
            ApplicationNumber = dto.ApplicationNumber,
            GroomName = dto.GroomName,
            BrideName = dto.BrideName,
            GroomPhone = dto.GroomPhone,
            BridePhone = dto.BridePhone,
            GroomAddress = dto.GroomAddress,
            BrideAddress = dto.BrideAddress,
            PresidentName = dto.PresidentName,
            Status = dto.Status,
            SubmittedDate = dto.SubmittedDate
        };
    }

}

