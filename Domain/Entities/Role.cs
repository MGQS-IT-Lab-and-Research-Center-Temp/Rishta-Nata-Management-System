namespace Domain.Entities;

public class Role
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string UpdatedBy { get; set; } = default!;

    public int HierarchyLevel { get; set; } // 1 = Jama'at Member, 2 = Jama'at President, 3 = Circuit President, 4 = National Rishtanata Secretary, 5 = Amir

    public ICollection<JamaatMember> Members { get; set; } = new List<JamaatMember>();
}
