using System;
using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class ParticipantInvitation : AuditableEntity
    {
        public Guid ApplicationId { get; set; }
        public FormApplication Application { get; set; } = null!;

        public Side Side { get; set; }
        public ParticipantRole ParticipantRole { get; set; }
        // nullable for guardians
        public int? WitnessOrder { get; set; }

        // Store hash of token
        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public InvitationStatus Status { get; set; }

        public DateTime? UsedAt { get; set; }
    }
}
