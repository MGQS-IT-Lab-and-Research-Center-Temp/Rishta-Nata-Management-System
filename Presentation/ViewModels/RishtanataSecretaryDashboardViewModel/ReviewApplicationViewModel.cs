using System;

namespace Presentation.ViewModels.RishtanataSecretaryDashboardViewModel;

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
