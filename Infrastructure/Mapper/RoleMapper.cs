using Domain.Entities;
using Infrastructure.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mapper;

    public static class RoleMapper
{
    public static RoleDto ToDto (Role role)
    {
        return new RoleDto
        {
            Name = role.Name,
            HierarchyLevel = role.HierarchyLevel,
            UpdatedBy = role.UpdatedBy
        };
    }

    public static Role toEntity (RoleDto roleDto)
    {
        return new Role
        { 
            Name = roleDto.Name,
            UpdatedBy = roleDto.UpdatedBy,
            HierarchyLevel = roleDto.HierarchyLevel
        };
    }
}

