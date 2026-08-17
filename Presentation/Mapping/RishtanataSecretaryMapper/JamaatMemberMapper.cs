using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels;

namespace Presentation.Mapping.RishtanataSecretaryMapper
{
    public static class JamaatMemberMapper
    {

        public static JamaatMemberViewModel ToViewModel (JamaatMemberDto dto)
        {
            return new JamaatMemberViewModel
            {
                Id = dto.Id,
                MembershipNumber = dto.MembershipNumber,
                FullName = dto.FullName,
                Gender = dto.Gender,
                JamaatName = dto.JamaatName,
                MaritalStatus = dto.MaritalStatus,
                PhoneNumber = dto.PhoneNumber, 
            };
        }
    }
}
