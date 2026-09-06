namespace Infrastructure.DTOs.MemberDashboard;

public class MemberDashboardDto
{
    public string MemberName { get; set; } = string.Empty;

    public SpouseInfoDto? CurrentSpouse { get; set; }

    public List<MarriageHistoryEntryDto> MarriageHistory { get; set; } = new();
}

public class SpouseInfoDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime MarriageDate { get; set; }
}

public class MarriageHistoryEntryDto
{
    public string SpouseName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
