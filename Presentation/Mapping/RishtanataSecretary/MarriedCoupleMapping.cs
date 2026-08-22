using Infrastructure.DTOs.MarriedCoupleDto;
using Presentation.ViewModels;

namespace Presentation.Mapping.RishtanataSecretary;

public static class MarriedCoupleMapping
{
    public static MarriedCoupleViewModel ToViewModel(MarriedCoupleDto dto)
    {
        return new MarriedCoupleViewModel
        {
            Id = dto.Id,
            ApplicationNumber = dto.ApplicationNumber,
            GroomName = dto.GroomName,
            GroomMembershipNo = dto.GroomMembershipNo,
            GroomDateOfBirth = dto.GroomDateOfBirth,
            BrideName = dto.BrideName,
            BrideMembershipNo = dto.BrideMembershipNo,
            BrideDateOfBirth = dto.BrideDateOfBirth,
            NikahDate = dto.NikahDate,
            Venue = dto.Venue,
            Status = dto.Status
        };
    }
}