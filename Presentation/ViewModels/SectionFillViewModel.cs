using System;
using Domain.Enums;

namespace Presentation.ViewModels;

public class SectionFillViewModel
{
    public string Token { get; set; } = string.Empty;
    public SectionType SectionType { get; set; }
    public string SectionLabel { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string BrideName { get; set; } = string.Empty;
    public string BridegroomName { get; set; } = string.Empty;

    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Tel { get; set; }
    public string? RelationToBride { get; set; }
    public bool IsMember { get; set; }
    public string? MemberMembershipNo { get; set; }

}