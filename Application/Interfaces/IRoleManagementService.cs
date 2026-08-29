using Domain.Entities;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces;

public interface IRoleAssignmentService

{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleManagementDto> GetRoleManagementAsync(Guid memberId);
    Task<(bool Success, string? Error)> AssignRoleAsync(Guid memberId, Guid roleId, string changedBy);
    Task<(bool Success, string? Error)> ResetToBaseRoleAsync(Guid memberId, string changedBy);
}