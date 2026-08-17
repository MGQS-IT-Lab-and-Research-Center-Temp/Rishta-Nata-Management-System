namespace Infrastructure.DTOs.Roles;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int HierarchyLevel { get; set; }
}