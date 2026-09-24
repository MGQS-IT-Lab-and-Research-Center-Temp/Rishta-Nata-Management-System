using Domain.Enums;

namespace Infrastructure.DTOs.RishtanataSecretaryDashboardDto
{

    public class ReviewApplicationDto
    {
        public Guid Id { get; set; }

        public string? ApplicationNumber { get; set; }

        public string? GroomName { get; set; }

        public string? BrideName { get; set; }

        public string? GroomPhone { get; set; }

        public string? BridePhone { get; set; }

        public string? GroomAddress { get; set; }

        public string? BrideAddress { get; set; }

        public string? JamaatName { get; set; }

        public string? PresidentName { get; set; }

        public string? PresidentRecommendation { get; set; }

        // Local Rishtanata Secretary per side (Gap 4); the groom side falls back to
        // the bride side on the same-Jama'at path.
        public string? BrideLocalRishtanataSecretaryName { get; set; }

        public string? BrideLocalRishtanataSecretarySignatureDate { get; set; }

        public string? GroomLocalRishtanataSecretaryName { get; set; }

        public string? GroomLocalRishtanataSecretarySignatureDate { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string? Status { get; set; }

        public bool IsApprovedByPresident { get; set; }

        /// <summary>The form's current review-chain stage, for the revert modal.</summary>
        public ApplicationStage? CurrentStage { get; set; }

        public string? OfficiatingImamMembershipNo { get; set; }

        public DateTime? ApprovedDateOfNikah { get; set; }
    }



}
