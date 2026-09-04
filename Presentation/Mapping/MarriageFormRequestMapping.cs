using Infrastructure.DTOs;
using Infrastructure.DTOs.BrideGroom;
using Presentation.Requests;

namespace Presentation.Mapping;

public static class MarriageFormRequestMapping
{
    public static BrideSectionDto ToDto(BrideSectionRequest request)
    {
        return new BrideSectionDto
        {
            MarriageApplicationId = request.MarriageApplicationId,
            BrideMembershipNo = request.BrideMembershipNo,
            BrideName = request.BrideName,
            BrideDateOfBirth = request.BrideDateOfBirth,
            BrideResidentOf = request.BrideResidentOf,
            BrideGenotype = request.BrideGenotype,
            BrideBloodGroup = request.BrideBloodGroup,
            BrideMaritalStatus = request.BrideMaritalStatus,
            BrideProposedDowerAmount = request.BrideProposedDowerAmount,
            BrideDowerAmountReceivedInCash = request.BrideDowerAmountReceivedInCash,
            BrideSignatureTel = request.BrideSignatureTel
        };
    }

    public static BridegroomSectionDto ToDto(BridegroomSectionRequest request)
    {
        return new BridegroomSectionDto
        {
            Id = request.Id,
            BridegroomMembershipNo = request.BridegroomMembershipNo,
            BridegroomName = request.BridegroomName,
            BridegroomDateOfBirth = request.BridegroomDateOfBirth,
            BridegroomResidentOf = request.BridegroomResidentOf,
            BridegroomGenotype = request.BridegroomGenotype,
            BridegroomBloodGroup = request.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash = request.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = request.BridegroomDowerAmountToBePaid,
            IsFirstNikah = request.IsFirstNikah,
            IsSecondThirdOrFourthNikah = request.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = request.FormerWifeIsDead,
            HasDivorcedFormerWife = request.HasDivorcedFormerWife,
            FormerWifeIsPresent = request.FormerWifeIsPresent,
            FormerWifeObtainedKhula = request.FormerWifeObtainedKhula,
            BridegroomSignatureTel = request.BridegroomSignatureTel
        };
    }
}
