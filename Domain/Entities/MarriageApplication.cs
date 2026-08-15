using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class MarriageApplication : AuditableEntity
    {
        // add fk to marriage application form
        //add fk to certificate
        //change name of the entity to only application
        public ApplicationStatus Status { get; set; }
        public Guid UserId { get; set; }
        public Certificate? Certificate { get; set; }
        public string? SerialNumber { get; set; }

    }
}
