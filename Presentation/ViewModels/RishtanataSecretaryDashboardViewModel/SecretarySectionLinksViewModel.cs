using Domain.Enums;
using Infrastructure.DTOs.SharedSection;

namespace Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

public class SecretarySectionLinksViewModel
{
    public Guid ApplicationId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public string BridegroomName { get; set; } = string.Empty;
    public MarriageFormStage FormStage { get; set; }
    public List<SectionLinkStatus> Items { get; set; } = new();
    public string? RegeneratedSection { get; set; }
    public string? RegeneratedUrl { get; set; }
}