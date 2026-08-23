using Infrastructure.DTOs.BrideGroom;
using Presentation.ViewModels;

namespace Presentation.Mapping.Bridegroom;

public static class BridegroomMapping
{
    public static BridegroomFormViewModel ToViewModel(BridegroomSectionDto dto)
    {
        return new BridegroomFormViewModel
        {
            BridegroomMembershipNo = dto.BridegroomMembershipNo,
            BridegroomName = dto.BridegroomName,
            BridegroomDateOfBirth = dto.BridegroomDateOfBirth,
            BridegroomResidentOf = dto.BridegroomResidentOf,
            BridegroomPhoneNumber = dto.BridegroomSignatureTel,
            BridegroomGenotype = dto.BridegroomGenotype,
            BridegroomBloodGroup = dto.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash = dto.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = dto.BridegroomDowerAmountToBePaid,
            IsFirstNikah = dto.IsFirstNikah,
            IsSecondThirdOrFourthNikah = dto.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = dto.FormerWifeIsDead,
            HasDivorcedFormerWife = dto.HasDivorcedFormerWife,
            FormerWifeIsPresent = dto.FormerWifeIsPresent,
            FormerWifeObtainedKhula = dto.FormerWifeObtainedKhula
        };
    }
}
