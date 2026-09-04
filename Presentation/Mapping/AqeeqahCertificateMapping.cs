using Infrastructure.DTOs.Certificates;
using Presentation.ViewModels;

namespace Presentation.Mapping;

public static class AqeeqahCertificateMapping
{
    public static AqeeqahCertificateViewModel ToViewModel(AqeeqahCertificateDto dto)
    {
        return new AqeeqahCertificateViewModel
        {
            Id = dto.Id,
            SerialNumber = dto.SerialNumber,
            ChildName = dto.ChildName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            PlaceOfBirth = dto.PlaceOfBirth,
            FatherName = dto.FatherName,
            MotherName = dto.MotherName,
            JamaatId = dto.JamaatId,
            JamaatName = dto.JamaatName,
            Address = dto.Address,
            OfficiatingMissionary = dto.OfficiatingMissionary,
            IssueDate = dto.IssueDate,
            IssuedByUserId = dto.IssuedByUserId,
            AqeeqahDate = dto.AqeeqahDate,
            AqeeqahLocation = dto.AqeeqahLocation,
            AnimalCount = dto.AnimalCount,
            CertificateFilePath = dto.CertificateFilePath
        };
    }

    public static AqeeqahCertificateDto ToDto(AqeeqahCertificateViewModel model)
    {
        return new AqeeqahCertificateDto
        {
            Id = model.Id,
            SerialNumber = model.SerialNumber,
            ChildName = model.ChildName,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            PlaceOfBirth = model.PlaceOfBirth,
            FatherName = model.FatherName,
            MotherName = model.MotherName,
            JamaatId = model.JamaatId,
            JamaatName = model.JamaatName,
            Address = model.Address,
            OfficiatingMissionary = model.OfficiatingMissionary,
            IssueDate = model.IssueDate,
            IssuedByUserId = model.IssuedByUserId,
            AqeeqahDate = model.AqeeqahDate,
            AqeeqahLocation = model.AqeeqahLocation,
            AnimalCount = model.AnimalCount,
            CertificateFilePath = model.CertificateFilePath
        };
    }
}
