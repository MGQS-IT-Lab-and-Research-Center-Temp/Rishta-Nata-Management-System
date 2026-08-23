namespace Presentation.ViewModels.RishtanataSecretaryDashboardViewModel
{
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
}
