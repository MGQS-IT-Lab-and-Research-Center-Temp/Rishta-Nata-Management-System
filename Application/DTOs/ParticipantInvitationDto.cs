using System;
using Domain.Enums;

namespace Application.DTOs
{
    public class ParticipantInvitationDto
    {
        public Guid InvitationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Side Side { get; set; }
        public ParticipantRole Role { get; set; }
        public int? WitnessOrder { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string? InvitationUrl { get; set; }
    }
}
