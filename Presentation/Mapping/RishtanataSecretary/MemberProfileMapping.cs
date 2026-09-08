using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels;

namespace Presentation.Mapping.RishtanataSecretary;

public static class MemberProfileMapping
{
    public static MemberProfileViewModel ToViewModel(MemberProfileDto dto)
    {
        return new MemberProfileViewModel
        {
            Id = dto.Id,
            FullName = dto.FullName,
            Title = dto.Title,
            Email = dto.Email,
            ChandaNo = dto.ChandaNo,
            WasiyatNo = dto.WasiyatNo,
            AuxillaryBodyName = dto.AuxillaryBodyName,
            DateOfBirth = dto.DateOfBirth,
            PhoneNo = dto.PhoneNo,
            JamaatName = dto.JamaatName,
            CircuitName = dto.CircuitName,
            Sex = dto.Sex,
            MaritalStatus = dto.MaritalStatus,
            Address = dto.Address,
            NextOfKinName = dto.NextOfKinName,
            NextOfKinPhoneNo = dto.NextOfKinPhoneNo,
            NextOfKinAddress = dto.NextOfKinAddress,
            Nationality = dto.Nationality,
            RoleName = dto.RoleName
        };
    }
}
