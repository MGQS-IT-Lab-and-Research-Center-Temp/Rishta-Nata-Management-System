using Domain.Abstractions;
using Domain.Enums;
using Domain.Entities;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{

        public class MarriageFormRejection : AuditableEntity
        {
                public Guid MarriageApplicationFormId { get; set; }

                public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;

                // Where the problem was found
                public ApplicationStage RejectedAtStage { get; set; }

                // Where it was sent back to
                public ApplicationStage RevertedToStage { get; set; }

                // Free-text reason for the rejection
                public string Reason { get; set; } = string.Empty;
        }
}