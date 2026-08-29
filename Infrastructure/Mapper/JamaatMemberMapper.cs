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
            Surname = entity.Surname,
            FirstName = entity.FirstName,
            Email = entity.Email,
            ChandaNo = entity.ChandaNo,
            WasiyatNo = entity.WasiyatNo ?? string.Empty,
            Title = entity.Title ?? string.Empty,
            AuxillaryBodyName = entity.AuxillaryBodyName ?? string.Empty,
            MiddleName = entity.MiddleName ?? string.Empty,
            MaidenName = entity.MaidenName ?? string.Empty,
            DateOfBirth = entity.DateOfBirth,
            PhoneNo = entity.PhoneNo ?? string.Empty,
            JamaatName = entity.JamaatName,
            CircuitName = entity.CircuitName,
            Sex = entity.Sex,
            MaritalStatus = entity.MaritalStatus ?? string.Empty,
            Address = entity.Address ?? string.Empty,
            NextOfKinPhoneNo = entity.NextOfKinPhoneNo ?? string.Empty,
            NextOfKinName = entity.NextOfKinName ?? string.Empty,
            NextOfKinAddress = entity.NextOfKinAddress ?? string.Empty,
            Nationality = entity.Nationality ?? string.Empty,
            RoleIds = entity.MemberRoles.Select(mr => mr.RoleId).ToList(),
            IsSystemDefault = entity.IsSystemDefault,
            NewRole = entity.NewRole,
        };
    }

    // NOTE: ToEntity no longer populates MemberRoles — role assignment must
    // go through RoleAssignmentService (AssignRoleAsync), not this mapper,
    // since roles are now a managed collection with audit fields
    // (AssignedAt/AssignedBy), not a plain field copy.
    public static JamaatMember ToEntity(JamaatMemberDto dto)
    {
        return new JamaatMember
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
            PhoneNo = dto.PhoneNo,
            JamaatName = dto.JamaatName,
            CircuitName = dto.CircuitName,
            Sex = dto.Sex,
            MaritalStatus = dto.MaritalStatus,
            Address = dto.Address,
            NextOfKinPhoneNo = dto.NextOfKinPhoneNo,
            NextOfKinName = dto.NextOfKinName,
            NextOfKinAddress = dto.NextOfKinAddress,
            Nationality = dto.Nationality,
            IsSystemDefault = dto.IsSystemDefault,
            NewRole = dto.NewRole,
        };
    }
}