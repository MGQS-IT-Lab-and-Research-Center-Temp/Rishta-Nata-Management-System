using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    public class Invitation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public InvitationTargetType TargetType { get; set; }

        // Link to the related marriage application
        public Guid? MarriageApplicationId { get; set; }
        public string? MarriageReferenceNumber { get; set; }

        // Link to the recipient JamaatMember (if applicable)
        public Guid? RecipientJamaatMemberId { get; set; }
        public string? RecipientMembershipNo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public bool Used { get; set; } = false;

        public string? CreatedBy { get; set; }
    }
}
