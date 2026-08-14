namespace Domain.Abstractions
{
    public abstract class AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt {  get; set; }
        public string? CreatedBy {  get; set; }
        public DateTime ModifiedAt {  get; set; }
        public string? ModifiedBy { get; set; }
    }
}
