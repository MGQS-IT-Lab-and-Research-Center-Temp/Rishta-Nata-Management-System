using Domain.Enums;

namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

/// <summary>
/// Read-side representation of the full state of a marriage application form
/// for display (Epic C3). One round trip gives the frontend everything it
/// needs to render "what's been completed so far".
///
/// Each section is null when that section has not been submitted yet; the
/// witness list is empty when no witness has submitted. CanCurrentUserEdit is
/// computed through IStageAuthorizationService — the same authorization logic
/// as Epic B — so the UI and the API can never disagree about who may act
/// (policy §7.3).
/// </summary>
public class MarriageApplicationFormDetailDto
{
    // ===== Shared fields =====
    public Guid FormId { get; set; }
    public Guid ApplicationId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public DateTime ProposedNikahDate { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }

    // ===== Workflow =====
    public ApplicationStage? CurrentStage { get; set; }

    // ===== Sections (null if not yet submitted) =====
    public BrideSectionDetailDto? Bride { get; set; }
    public BridegroomSectionDetailDto? Bridegroom { get; set; }
    public GuardianSectionDetailDto? Guardian { get; set; }
    public RepresentativeSectionDetailDto? Representative { get; set; }

    /// <summary>Witnesses that have submitted, in paper-form order (One, Two).</summary>
    public IReadOnlyList<WitnessDetailDto> Witnesses { get; set; } = Array.Empty<WitnessDetailDto>();

    public OfficiatingImamSectionDetailDto? OfficiatingImam { get; set; }
    public JamaatPresidentSectionDetailDto? JamaatPresident { get; set; }
    public RishtanataSecretarySectionDetailDto? NationalRishtanataSecretary { get; set; }
    public AmirApprovalSectionDetailDto? AmirApproval { get; set; }

    // ===== Rejection history =====
    public IReadOnlyList<RejectionHistoryItemDto> Rejections { get; set; } = Array.Empty<RejectionHistoryItemDto>();

    // ===== Authorization (Epic B parity) =====
    public bool CanCurrentUserEdit { get; set; }
}