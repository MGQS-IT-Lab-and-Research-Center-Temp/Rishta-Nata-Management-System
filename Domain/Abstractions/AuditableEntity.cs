namespace Domain.Abstractions;

public abstract class AuditableEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy {  get; set; }
    public DateTime ModifiedAt {  get; set; }
    public Guid? ModifiedBy { get; set; }
}
