using Domain.Abstractions;

namespace Domain.Entities
{
    public class Bride : AuditableEntity
    {
        public Guid MarriageApplicationId { get; set; }
        public MarriageApplication MarriageApplication { get; set; } = null!;
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
