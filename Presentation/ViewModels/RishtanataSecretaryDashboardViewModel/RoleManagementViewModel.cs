using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.DTOs.Roles;
namespace Presentation.ViewModels.RishtanataSecretaryDashboardViewModel
{
    public class RoleManagementViewModel
    {
        public Guid MemberId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ChandaNo { get; set; } = string.Empty;
        public IEnumerable<RoleDto> CurrentRoles { get; set; } = new List<RoleDto>();
        public IEnumerable<RoleDto> AvailableRoles { get; set; } = new List<RoleDto>();
        public bool IsAtBaseRole => CurrentRoles.Count() == 1 && CurrentRoles.First().HierarchyLevel == 1;
    }
}