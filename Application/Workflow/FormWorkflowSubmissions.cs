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
/// section of the paper form.
/// </summary>
public sealed record JamaatPresidentVerificationSubmission(
    string Name,
    string Tel,
    string SignatureDate);

/// <summary>
/// Payload for SubmitRishtanataRecommendationAsync. Maps to the
/// "National Rishtanata Secretary" section of the paper form. The secretary also
/// designates the officiating imam (ChandaNo) and may record an agreed-date
/// change communicated by the partners.
/// </summary>
public sealed record RishtanataRecommendationSubmission(
    string WakeelName,
    string WakeelDeclaration,
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