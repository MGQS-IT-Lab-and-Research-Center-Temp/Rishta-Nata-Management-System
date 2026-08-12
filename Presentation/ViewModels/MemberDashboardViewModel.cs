using System;
using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class MemberDashboardViewModel
    {
        public string MemberName { get; set; } = "Azeem";

        public SpouseInfo? CurrentSpouse { get; set; }

        public List<MarriageHistoryEntry> MarriageHistory { get; set; } = new();

        // Dummy fields for the non-functional "Apply for Nikah" modal form.
        public NikahApplicationForm NikahForm { get; set; } = new();
    }

    public class SpouseInfo
    {
        public string Name { get; set; } = string.Empty;
        public DateTime MarriageDate { get; set; }
    }

    public class MarriageHistoryEntry
    {
        public string SpouseName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // e.g. "Married", "Divorced"
        public DateTime Date { get; set; }
    }

    public class NikahApplicationForm
    {
        public string ProspectiveSpouseName { get; set; } = string.Empty;
        public string ProspectiveSpouseGuardian { get; set; } = string.Empty;
        public DateTime ProposedDate { get; set; } = DateTime.Today;
        public string Notes { get; set; } = string.Empty;
    }
}