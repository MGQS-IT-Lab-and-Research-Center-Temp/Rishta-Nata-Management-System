using Infrastructure.DTOs.Roles;

namespace Presentation.ViewModels
{
    public class RishtanataSecretaryDashboardViewModel
    {
        public string? SecretaryName { get; set; }

        public int PendingApprovals { get; set; }

        public int ApprovedApplications { get; set; }

        public int RejectedApplications { get; set; }

        public int MarriedCouples { get; set; }

        public int TotalMembers { get; set; }

        public List<PendingApprovalViewModel> PendingApplications { get; set; }
            = new();

        public List<RecentActivityViewModel> RecentActivities { get; set; }
            = new();
    }




    public class ReviewApplicationViewModel
    {
        public Guid Id { get; set; }

        public string? ApplicationNumber { get; set; }

        public string? GroomName { get; set; }

        public string? BrideName { get; set; }

        public string? GroomPhone { get; set; }

        public string? BridePhone { get; set; }

        public string? GroomAddress { get; set; }

        public string? BrideAddress { get; set; }

        public string? JamaatName { get; set; }

        public string? PresidentName { get; set; }

        public string? PresidentRecommendation { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string? Status { get; set; }

        public bool IsApprovedByPresident { get; set; }
    }




    public class PendingApprovalViewModel
    {
        public Guid Id { get; set; }

        public string? ApplicationNumber { get; set; }

        public string? GroomName { get; set; }

        public string? BrideName { get; set; }

        public string? JamaatName { get; set; }

        public string? PresidentName { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string? PresidentRecommendation { get; set; }

        public string? Status { get; set; }
    }

    public class MarriedCoupleViewModel
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;

        public string GroomName { get; set; } = string.Empty;
        public string GroomMembershipNo { get; set; } = string.Empty;
        public DateTime GroomDateOfBirth { get; set; }

        public string BrideName { get; set; } = string.Empty;
        public string BrideMembershipNo { get; set; } = string.Empty;
        public DateTime BrideDateOfBirth { get; set; }

        public DateTime NikahDate { get; set; }
        public string Venue { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }

    public class JamaatMemberViewModel
    {
        public Guid Id { get; set; }

        public string? MembershipNumber { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? JamaatName { get; set; }

        public string? MaritalStatus { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class RoleManagementViewModel
    {
        public Guid MemberId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ChandaNo { get; set; } = string.Empty;

        public RoleDto CurrentRole { get; set; } = default!;
        public IEnumerable<RoleDto> AvailableRoles { get; set; }

        public bool IsAtBaseRole => CurrentRole.HierarchyLevel == 1;
    }


}
