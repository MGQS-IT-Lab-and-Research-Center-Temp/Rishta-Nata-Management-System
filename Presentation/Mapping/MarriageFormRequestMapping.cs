using Infrastructure.DTOs;
using Infrastructure.DTOs.BrideGroom;
using Presentation.Requests;
using Presentation.ViewModels;

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
            CurrentNikahOrdinal = request.CurrentNikahOrdinal,
            FormerWifeIsDead = request.FormerWifeIsDead,
            HasDivorcedFormerWife = request.HasDivorcedFormerWife,
            FormerWifeIsPresent = request.FormerWifeIsPresent,
            FormerWifeObtainedKhula = request.FormerWifeObtainedKhula,
            BridegroomSignatureTel = request.BridegroomSignatureTel
        };
    }

    public static BrideSectionDto ToBrideDto(ContinueApplicationViewModel model) =>
        new BrideSectionDto
        {
            MarriageApplicationId = model.Id,
            BrideMembershipNo = model.MembershipNo,
            BrideName = model.Name,
            BrideDateOfBirth = model.DateOfBirth,
            BrideResidentOf = model.ResidentOf,
            BrideGenotype = model.Genotype,
            BrideBloodGroup = model.BloodGroup,
            BrideMaritalStatus = model.MaritalStatus,
            BrideDivorceEvidence = model.BrideDivorceEvidence,
            BrideProposedDowerAmount = model.ProposedDowerAmount,
            BrideDowerAmountReceivedInCash = model.DowerAmountReceivedInCash,
            BrideSignatureTel = model.Phone
        };

    public static BridegroomSectionDto ToBridegroomDto(ContinueApplicationViewModel model) =>
        new BridegroomSectionDto
        {
            Id = model.Id,
            BridegroomMembershipNo = model.MembershipNo,
            BridegroomName = model.Name,
            BridegroomDateOfBirth = model.DateOfBirth,
            BridegroomResidentOf = model.ResidentOf,
            BridegroomGenotype = model.Genotype,
            BridegroomBloodGroup = model.BloodGroup,
            BridegroomDowerAmountPaidInCash = model.DowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = model.DowerAmountToBePaid,
            IsFirstNikah = model.IsFirstNikah,
            CurrentNikahOrdinal = model.CurrentNikahOrdinal,
            FormerWifeIsDead = model.FormerWifeIsDead,
            HasDivorcedFormerWife = model.HasDivorcedFormerWife,
            BridegroomDivorceEvidence = model.BridegroomDivorceEvidence,
            FormerWifeIsPresent = model.FormerWifeIsPresent,
            FormerWifeObtainedKhula = model.FormerWifeObtainedKhula,
            BridegroomSignatureTel = model.Phone
        };
}
