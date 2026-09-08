using Domain.Enums;
using Infrastructure.DTOs.SharedSection;

namespace Presentation.ViewModels;

public class SectionLinksViewModel
{
    public Guid ApplicationId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public string BridegroomName { get; set; } = string.Empty;
    public MarriageFormStage FormStage { get; set; }
    public List<SectionLinkStatus> Items { get; set; } = new();
}