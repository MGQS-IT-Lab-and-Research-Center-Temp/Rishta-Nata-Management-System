using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class MarriageApplicationFormDto
    {
        public Guid Id { get; set; }

        // Application
        public Guid MarriageApplicationId { get; set; }

        // Bride
        public Guid BrideId { get; set; }

        // Groom
        public Guid GroomId { get; set; }

        // Bride's Father
        public Guid? BrideFatherId { get; set; }

        // Guardian / Waliyy
        public Guid? GuardianId { get; set; }

        // Representative / Wakeel
        public Guid? RepresentativeId { get; set; }

        // Witnesses
        public List<Guid> WitnessIds { get; set; } = new();

        // Verification
        public Guid? VerificationId { get; set; }

        // Approval
        public Guid? ApprovalId { get; set; }

        // Status
        public string Status { get; set; } = string.Empty;
    }
}
