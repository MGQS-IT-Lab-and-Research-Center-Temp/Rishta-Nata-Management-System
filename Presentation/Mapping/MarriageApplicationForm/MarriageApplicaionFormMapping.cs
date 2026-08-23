using Infrastructure.DTOs.Bride;
using Presentation.ViewModels.Bride;

namespace Presentation.Mapping.MarriageApplicationForm;

public static class MarriageApplicationFormMapping
{
    public static BrideFormSectionViewModel ToViewModel(BrideDto dto)
    {
        return new BrideFormSectionViewModel
        {
            Id = dto.Id,

            MarriageApplicationFormId =
                dto.MarriageApplicationFormId,

            MembershipNo =
                dto.MembershipNo,

            Name =
                dto.Name,

            DateOfBirth =
                dto.DateOfBirth,

            ResidentOf =
                dto.ResidentOf,

            Genotype =
                dto.Genotype,

            BloodGroup =
                dto.BloodGroup,

            MaritalStatus =
                dto.MaritalStatus,

            ProposedDowerAmount =
                dto.ProposedDowerAmount,

            DowerAmountReceivedInCash =
                dto.DowerAmountReceivedInCash,

            SignatureTel =
                dto.SignatureTel,

            FatherName =
                dto.FatherName
        };
    }
}