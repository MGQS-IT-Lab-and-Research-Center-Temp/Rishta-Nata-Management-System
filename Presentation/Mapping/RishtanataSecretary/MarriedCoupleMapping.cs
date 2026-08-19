using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Presentation.ViewModels;

namespace Presentation.Mapping.RishtanataSecretary;

public static class MarriedCoupleMapping
{
    public static MarriedCoupleViewModel ToViewModel(MarriedCoupleDto dto)
    {
        return new MarriedCoupleViewModel
        {
            Id = dto.Id,
            CertificateNumber = dto.CertificateNumber.ToString(),
            HusbandName = dto.HusbandName,
            WifeName = dto.WifeName,
            JamaatName = dto.JamaatName,
            MarriageDate = dto.MarriageDate,
            Status = dto.Status
        };
    }
}
