

namespace Application.DTOs
{
    public class RishtanataSecretaryDashboardDto
    {
        public string? SecretaryName { get; set; }

        public int PendingApprovals { get; set; }

        public int ApprovedApplications { get; set; }

        public int RejectedApplications { get; set; }

        public int MarriedCouples { get; set; }

        public int TotalMembers { get; set; }

        public List<PendingApprovalDto> PendingApplications { get; set; }
            = new();

        public List<RecentActivityDto> RecentActivities { get; set; }
            = new();
    }




    public class ReviewApplicationDto
    {
        public int Id { get; set; }

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




    public class PendingApprovalDto
    {
        public int Id { get; set; }

        public string? ApplicationNumber { get; set; }

        public string? GroomName { get; set; }

        public string? BrideName { get; set; }

        public string? JamaatName { get; set; }

        public string? PresidentName { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string? PresidentRecommendation { get; set; }

        public string? Status { get; set; }
    }


    public class MarriedCoupleDto
    {
        public int Id { get; set; }

        public string? CertificateNumber { get; set; }

        public string? HusbandName { get; set; }

        public string? WifeName { get; set; }

        public string? JamaatName { get; set; }

        public DateTime MarriageDate { get; set; }

        public string? Status { get; set; }
    }

    public class JamaatMemberDto
    {
        public int Id { get; set; }

        public string? MemberNumber { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? JamaatName { get; set; }

        public string? MaritalStatus { get; set; }

        public string? Occupation { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class RecentActivityDto
    {
        public int Id { get; set; }
        public string? ActivityType { get; set; }
        public string? Description { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}
