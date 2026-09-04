using System;

namespace Presentation.Requests;

public class RoleRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int HierarchyLevel { get; set; }
}
