//using Domain.Entities;
//using Infrastructure.DTOs.Bride;
//using Presentation.ViewModels;

//namespace Presentation.Mapping.MarriageApplicationForm;

//public static class MarriageApplicationFormMapping
//{
//    public static BrideViewModel ToViewModel(BrideDto dto)
//    {
//        return new BrideViewModel
//        {
//            Id = dto.Id,
//            MarriageApplicationFormId = dto.MarriageApplicationFormId,
//            MembershipNo = dto.MembershipNo,
//            Name = dto.Name,
//            DateOfBirth = dto.DateOfBirth,
//            ResidentOf = dto.ResidentOf,
//            Genotype = dto.Genotype,
//            BloodGroup = dto.BloodGroup,
//            MaritalStatus = dto.MaritalStatus,
//            ProposedDowerAmount = dto.ProposedDowerAmount,
//            DowerAmountReceivedInCash = dto.DowerAmountReceivedInCash,
//            SignatureTel = dto.SignatureTel,
//            FatherName = dto.FatherName
//        };
//    }
//}


using Infrastructure.DTOs.Bride;
using Presentation.ViewModels.Bride;

namespace Presentation.Mapping.MarriageApplicationForm;

public static class MarriageApplicationFormMapping
{
    public static BrideViewModel ToViewModel(BrideDto dto)
    {
        return new BrideViewModel
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