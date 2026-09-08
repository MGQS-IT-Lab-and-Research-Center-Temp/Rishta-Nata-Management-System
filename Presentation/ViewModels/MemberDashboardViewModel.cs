using System;
using System.Collections.Generic;

namespace Presentation.ViewModels;

public class MemberDashboardViewModel
{
    public string MemberName { get; set; } = string.Empty;

    public SpouseInfo? CurrentSpouse { get; set; }

    public List<MarriageHistoryEntry> MarriageHistory { get; set; } = new();

    public MemberApplicationViewModel? ActiveApplication { get; set; }
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