using Infrastructure.DTOs.BrideGuardian;
using Presentation.ViewModel;

namespace Presentation.Mapping;

public static class BrideGuardianViewModelMapper
{
    public static BrideGuardianViewModel ToViewModel(
        MarriageApplicationFormViewModel application,
        string referenceNumber)
    {
        return new BrideGuardianViewModel
        {
            MarriageApplicationId = application.MarriageApplicationId,
            ReferenceNumber = referenceNumber,
            BrideName = application.BrideName,
            BrideFatherName = application.BrideFatherName,
            BrideDateOfBirth = application.BrideDateOfBirth,
            BrideResidentOf = application.BrideResidentOf,
            BrideGenotype = application.BrideGenotype,
            BrideBloodGroup = application.BrideBloodGroup,
            BrideMaritalStatus = application.BrideMaritalStatus,
            BrideProposedDowerAmount = application.BrideProposedDowerAmount,
            BrideDowerAmountReceivedInCash = application.BrideDowerAmountReceivedInCash,
            BridegroomName = application.BridegroomName,
            BridegroomFatherName = application.BridegroomFatherName,
            BridegroomDateOfBirth = application.BridegroomDateOfBirth,
            BridegroomResidentOf = application.BridegroomResidentOf
        };
    }

    public static BrideGuardianDto ToDto(BrideGuardianViewModel model)
    {
        return new BrideGuardianDto
        {
            MarriageApplicationId = model.MarriageApplicationId,
            ReferenceNumber = model.ReferenceNumber,
            GuardianName = model.GuardianName,
            GuardianRelationToBride = model.GuardianRelationToBride,
            GuardianAddress = model.GuardianAddress,
            GuardianTel = model.GuardianTel,
            GuardianSignatureDate = model.GuardianSignatureDate
        };
    }

    public static BrideGuardianViewModel ToDetailsViewModel(BrideGuardianDto dto)
    {
        return new BrideGuardianViewModel
        {
            MarriageApplicationId = dto.MarriageApplicationId,
            ReferenceNumber = dto.ReferenceNumber,
            GuardianName = dto.GuardianName,
            GuardianRelationToBride = dto.GuardianRelationToBride,
            GuardianAddress = dto.GuardianAddress,
            GuardianTel = dto.GuardianTel,
            GuardianSignatureDate = dto.GuardianSignatureDate
        };
    }
}