using Infrastructure.DTOs.JamaatMember;
using Presentation.ViewModels.JamaatMember;

namespace Presentation.Mapping.JamaatMember
{
    public static class JamaatMemberMapping
    {
        public static JamaatMemberVM ToViewModel(JamaatMemberDto dto)
        {
            return new JamaatMemberVM
            {
                Id = dto.Id,
                Surname = dto.Surname,
                FirstName = dto.FirstName,
                Email = dto.Email,
                ChandaNo = dto.ChandaNo,
                WasiyatNo = dto.WasiyatNo,
                Title = dto.Title,
                AuxillaryBodyName = dto.AuxillaryBodyName,
                MiddleName = dto.MiddleName,
                MaidenName = dto.MaidenName,
                DateOfBirth = dto.DateOfBirth,

                // ✅ fixed property names
                PhoneNumber = dto.PhoneNo,
                JamaatName = dto.JamaatName,
                CircuitName = dto.CircuitName,
                Gender = dto.Sex,
                MaritalStatus = dto.MaritalStatus,
                Address = dto.Address,

                NextOfKinPhoneNo = dto.NextOfKinPhoneNo,
                NextOfKinName = dto.NextOfKinName,
                NextOfKinAddress = dto.NextOfKinAddress,
                Nationality = dto.Nationality,

                RoleId = dto.RoleId,
                IsSystemDefault = dto.IsSystemDefault,
                NewRole = dto.NewRole,

                
                MemberNumber = dto.MemberNumber,
                Occupation = dto.Occupation
            };
        }
    }
}
