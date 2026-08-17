using Domain.Entities;
using Infrastructure.DTOs.JamaatMember;

namespace Infrastructure.Mapper;

public static class JamaatMemberMapper
{
    public static JamaatMemberDto ToDto(JamaatMember entity)
    {
        return new JamaatMemberDto
        {
            Id = entity.Id,
            Surname = entity.surname,
            FirstName = entity.firstName,
            Email = entity.email,
            ChandaNo = entity.chandaNo,
            WasiyatNo = entity.wasiyatNo ?? string.Empty,
            Title = entity.title ?? string.Empty,
            AuxillaryBodyName = entity.auxillaryBodyName ?? string.Empty,
            MiddleName = entity.middleName ?? string.Empty,
            MaidenName = entity.maidenName ?? string.Empty,
            DateOfBirth = entity.dateOfBirth,
            PhoneNo = entity.phoneNo ?? string.Empty,
            JamaatName = entity.jamaatName,
            CircuitName = entity.circuitName,
            Sex = entity.sex,
            MaritalStatus = entity.maritalStatus ?? string.Empty,
            Address = entity.address ?? string.Empty,
            NextOfKinPhoneNo = entity.nextOfKinPhoneNo ?? string.Empty,
            NextOfKinName = entity.nextOfKinName ?? string.Empty,
            NextOfKinAddress = entity.nextOfKinAddress ?? string.Empty,
            Nationality = entity.nationality ?? string.Empty,
            RoleId = entity.RoleId,
            IsSystemDefault = entity.IsSystemDefault,
            NewRole = entity.NewRole,
        };
    }

    public static JamaatMember ToEntity(JamaatMemberDto dto)
    {
        return new JamaatMember
        {
            Id = dto.Id,
            surname = dto.Surname,
            firstName = dto.FirstName,
            email = dto.Email,
            chandaNo = dto.ChandaNo,
            wasiyatNo = dto.WasiyatNo,
            title = dto.Title,
            auxillaryBodyName = dto.AuxillaryBodyName,
            middleName = dto.MiddleName,
            maidenName = dto.MaidenName,
            dateOfBirth = dto.DateOfBirth,
            phoneNo = dto.PhoneNo,
            jamaatName = dto.JamaatName,
            circuitName = dto.CircuitName,
            sex = dto.Sex,
            maritalStatus = dto.MaritalStatus,
            address = dto.Address,
            nextOfKinPhoneNo = dto.NextOfKinPhoneNo,
            nextOfKinName = dto.NextOfKinName,
            nextOfKinAddress = dto.NextOfKinAddress,
            nationality = dto.Nationality,
            RoleId = dto.RoleId,
            IsSystemDefault = dto.IsSystemDefault,
            NewRole = dto.NewRole,
        };
    }
}