namespace Application.Workflow;

/// <summary>
/// Payload for SubmitImamVerificationAsync (backlog D3). Maps to the
/// "Officiating Imam" section of the paper form.
/// </summary>
public sealed record ImamVerificationSubmission(
    string Name,
    string AddressJamaat,
    string Tel,
    string SignatureDate);

/// <summary>
/// Payload for SubmitJamaatPresidentVerificationAsync (backlog D3). Maps to
/// the "Jamaat President" section of the paper form.
/// </summary>
public sealed record JamaatPresidentVerificationSubmission(
    string Name,
    string Tel,
    string SignatureDate);

/// <summary>
/// Payload for SubmitRishtanataRecommendationAsync (backlog D3). Maps to the
/// "National Rishtanata Secretary" section of the paper form.
/// </summary>
public sealed record RishtanataRecommendationSubmission(
    string WakeelName,
    string WakeelDeclaration,
    string SignatureDate);

/// <summary>
/// Payload for ApproveByAmirAsync (backlog D3). Maps to the
/// "National Amir / Missionary In-charge" section of the paper form.
/// The approved Nikah date is recorded on both the section row and the form.
/// </summary>
public sealed record AmirApprovalSubmission(
    DateTime? ApprovedDateOfNikah,
    string SignatureDate);