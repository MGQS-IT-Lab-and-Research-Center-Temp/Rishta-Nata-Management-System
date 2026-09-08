using Domain.Entities;
using Infrastructure.DTOs.MarriedCoupleDto;

namespace Infrastructure.Mapper;

public static class MarriedCoupleMapper
{
    public static MarriedCoupleDto ToDto(MarriageApplicationForm entity)
    {
        return new MarriedCoupleDto
        {
            Id = entity.MarriageApplicationId,
            ApplicationNumber = entity.ReferenceNumber,
            GroomName = entity.BridegroomName,
            GroomMembershipNo = entity.BridegroomMembershipNo,
            GroomDateOfBirth = entity.BridegroomDateOfBirth,
            BrideName = entity.BrideName,
            BrideMembershipNo = entity.BrideMembershipNo,
            BrideDateOfBirth = entity.BrideDateOfBirth,
            NikahDate = entity.ApprovedDateOfNikah ?? entity.ProposedNikahDate,
            Venue = entity.Venue,
            Status = entity.MarriageApplication.Status.ToString()
        };
    }

    public static MarriageApplicationForm ToEntity(MarriedCoupleDto dto)
    {
        return new MarriageApplicationForm
        {
            MarriageApplicationId = dto.Id,
            ReferenceNumber = dto.ApplicationNumber,
            BridegroomName = dto.GroomName,
            BridegroomMembershipNo = dto.GroomMembershipNo,
            BridegroomDateOfBirth = dto.GroomDateOfBirth,
            BrideName = dto.BrideName,
            BrideMembershipNo = dto.BrideMembershipNo,
            BrideDateOfBirth = dto.BrideDateOfBirth,
            ProposedNikahDate = dto.NikahDate,
            Venue = dto.Venue
        };
    }
}