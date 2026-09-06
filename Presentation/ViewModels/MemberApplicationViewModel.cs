using System;

namespace Presentation.ViewModels;

public class MemberApplicationViewModel
{
    public Guid Id { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string SpouseName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime SubmittedDate { get; set; }
}
