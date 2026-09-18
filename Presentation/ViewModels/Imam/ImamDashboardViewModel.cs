namespace Presentation.ViewModels.Imam;

public class ImamDashboardViewModel
{
    public IReadOnlyList<ImamPendingItem> PendingSignoffs { get; set; }
        = Array.Empty<ImamPendingItem>();

    public sealed class ImamPendingItem
    {
        public Guid FormId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Couple => $"{BrideName} & {BridegroomName}".Trim();
        public string BrideName { get; set; } = string.Empty;
        public string BridegroomName { get; set; } = string.Empty;
        public DateTime? ApprovedDateOfNikah { get; set; }
        public string Venue { get; set; } = string.Empty;
        public string SignatureDate { get; set; } = string.Empty;
        public bool Signed => !string.IsNullOrWhiteSpace(SignatureDate);
    }
}