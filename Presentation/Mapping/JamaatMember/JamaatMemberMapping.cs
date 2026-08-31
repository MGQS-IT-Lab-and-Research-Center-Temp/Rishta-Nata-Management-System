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
                RoleIds = dto.RoleIds,
                IsSystemDefault = dto.IsSystemDefault,
                NewRole = dto.NewRole,
                MemberNumber = dto.MemberNumber,
                Occupation = dto.Occupation
            };
        }
        public static List<JamaatMemberVM> ToViewModelList(IEnumerable<JamaatMemberDto> dtos)
        {
            return dtos?.Select(ToViewModel).ToList() ?? new List<JamaatMemberVM>();
        }
        public static JamaatMemberDto ToDto(JamaatMemberVM vm)
        {
            return new JamaatMemberDto
            {
                Id = vm.Id,
                Surname = vm.Surname,
                FirstName = vm.FirstName,
                Email = vm.Email,
                ChandaNo = vm.ChandaNo,
                WasiyatNo = vm.WasiyatNo,
                Title = vm.Title,
                AuxillaryBodyName = vm.AuxillaryBodyName,
                MiddleName = vm.MiddleName,
                MaidenName = vm.MaidenName,
                DateOfBirth = vm.DateOfBirth,
                PhoneNo = vm.PhoneNumber,
                JamaatName = vm.JamaatName,
                CircuitName = vm.CircuitName,
                Sex = vm.Gender,
                MaritalStatus = vm.MaritalStatus,
                Address = vm.Address,
                NextOfKinPhoneNo = vm.NextOfKinPhoneNo,
                NextOfKinName = vm.NextOfKinName,
                NextOfKinAddress = vm.NextOfKinAddress,
                Nationality = vm.Nationality,
                RoleIds = vm.RoleIds,
                IsSystemDefault = vm.IsSystemDefault,
                NewRole = vm.NewRole
            };
        }
    }
}