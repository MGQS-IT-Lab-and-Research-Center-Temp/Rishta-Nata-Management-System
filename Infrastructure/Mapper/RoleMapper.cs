using Domain.Entities;
using Infrastructure.DTOs.Roles;

namespace Infrastructure.Mapper;

public static class RoleMapper
{
    public static RoleDto ToDto(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            HierarchyLevel = role.HierarchyLevel,
            UpdatedBy = role.UpdatedBy
        };
    }

    public static Role toEntity(RoleDto roleDto)
    {
        return new Role
        {
            Id = roleDto.Id,
            Name = roleDto.Name,
            UpdatedBy = roleDto.UpdatedBy,
            HierarchyLevel = roleDto.HierarchyLevel
        };
    }
}




