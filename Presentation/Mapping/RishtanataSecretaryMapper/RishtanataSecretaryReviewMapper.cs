using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels;


namespace Presentation.Mapping.RishtanataSecretaryMapper;

    public static class RishtanataSecretaryReviewMapper
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

