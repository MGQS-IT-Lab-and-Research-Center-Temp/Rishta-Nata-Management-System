namespace Application.Workflow;

/// <summary>
/// Payload for SubmitImamSignoffAsync. Maps to the "Officiating Imam" section of
/// the paper form. The imam signs AFTER the ceremony; the SignatureDate is the
/// sign-off date confirming the ceremony took place.
/// </summary>
public sealed record ImamSignoffSubmission(
    string Name,
    string AddressJamaat,
    string Tel,
    string SignatureDate);

/// <summary>
/// Payload for SubmitJamaatPresidentVerificationAsync and
/// SubmitGroomJamaatPresidentVerificationAsync. Maps to the "Jamaat President"
/// section of the paper form, plus that side's "Local Rishtanata Secretary"
/// block, which the president records when signing (Gap 4). All three
/// LocalRishtanataSecretary* values are required to sign; the signature date
/// is "yyyy-MM-dd". Gap 7 adds the president's attestations: Bride (bride's
/// president), Groom (groom's president, or the bride's president when the
/// partners share a Jama'at), and the bride-side GuardianIsBonafide and
/// BrideSignedFreely ticks, both required on the bride side.
/// </summary>
public sealed record JamaatPresidentVerificationSubmission(
    string Name,
    string Tel,
    string SignatureDate,
    string LocalRishtanataSecretaryName,
    string LocalRishtanataSecretaryTel,
    string LocalRishtanataSecretarySignatureDate,
    PartnerAttestationSubmission? Bride,
    PartnerAttestationSubmission? Groom,
    bool GuardianIsBonafide,
    bool BrideSignedFreely);

/// <summary>
/// The president's attestation for one partner (Gap 7). IsBornAhmadi is
/// required to sign; YearsAsAhmadi (0–120) is required when it is false and
/// ignored when true; MarriageReason is optional (max 200).
/// </summary>
public sealed record PartnerAttestationSubmission(
    bool? IsBornAhmadi,
    int? YearsAsAhmadi,
    string? MarriageReason);

/// <summary>
/// Payload for SubmitRishtanataRecommendationAsync. Maps to the
/// "National Rishtanata Secretary" section of the paper form. The secretary also
/// designates the officiating imam (ChandaNo) and may record an agreed-date
/// change communicated by the partners.
/// </summary>
public sealed record RishtanataRecommendationSubmission(
    string Name,
    string Recommendation,
    string SignatureDate,
    string OfficiatingImamMembershipNo,
    DateTime? ApprovedDateOfNikah);

/// <summary>
/// Payload for ApproveByAmirAsync. Maps to the "National Amir / Missionary
/// In-charge" section of the paper form. The Amir no longer invents a Nikah date:
/// the agreed date comes from the couple (reconciled via the Rishtanata office).
/// </summary>
public sealed record AmirApprovalSubmission(
    string SignatureDate);