using Domain.Entities;
using Infrastructure.DTOs.BrideGroom;

namespace Infrastructure.Mapper;

public static class BrideGroomMapper
{
    public static BridegroomSectionDto ToDto(BridegroomFormSection entity)
    {
        return new BridegroomSectionDto
        {
            Id = entity.Id,
            BridegroomMembershipNo = entity.BridegroomMembershipNo,
            BridegroomName = entity.BridegroomName,
            BridegroomDateOfBirth = entity.BridegroomDateOfBirth,
            BridegroomResidentOf = entity.BridegroomResidentOf,
            BridegroomGenotype = entity.BridegroomGenotype,
            BridegroomBloodGroup = entity.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash = entity.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = entity.BridegroomDowerAmountToBePaid,
            IsFirstNikah = entity.IsFirstNikah,
            IsSecondThirdOrFourthNikah = entity.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = entity.FormerWifeIsDead,
            HasDivorcedFormerWife = entity.HasDivorcedFormerWife,
            FormerWifeIsPresent = entity.FormerWifeIsPresent,
            FormerWifeObtainedKhula = entity.FormerWifeObtainedKhula,
            BridegroomSignatureTel = entity.BridegroomSignatureTel
        };
    }

    public static BridegroomFormSection ToEntity(BridegroomSectionDto dto)
    {
        return new BridegroomFormSection
        {
            Id = dto.Id,
            BridegroomMembershipNo = dto.BridegroomMembershipNo,
            BridegroomName = dto.BridegroomName,
            BridegroomDateOfBirth = dto.BridegroomDateOfBirth,
            BridegroomResidentOf = dto.BridegroomResidentOf,
            BridegroomGenotype = dto.BridegroomGenotype,
            BridegroomBloodGroup = dto.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash = dto.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = dto.BridegroomDowerAmountToBePaid,
            IsFirstNikah = dto.IsFirstNikah,
            IsSecondThirdOrFourthNikah = dto.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = dto.FormerWifeIsDead,
            HasDivorcedFormerWife = dto.HasDivorcedFormerWife,
            FormerWifeIsPresent = dto.FormerWifeIsPresent,
            FormerWifeObtainedKhula = dto.FormerWifeObtainedKhula,
            BridegroomSignatureTel = dto.BridegroomSignatureTel
        };
    }
}
