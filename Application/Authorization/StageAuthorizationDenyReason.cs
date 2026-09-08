namespace Application.Authorization;

// This enum is used as a reason for stage-authorization denials
public enum StageAuthorizationDenyReason
{
    // The caller supplied no usable user identity (empty user id)
    NoMembershipClaim = 1,

    // The user id resolves to no known JamaatMember record
     UnknownMember = 2,

    // Member exists but is not the party/office-holder responsible for the target stage on this application.
    WrongRole = 3,

    // Identity fallback matched multiple member records — treated as unresolved rather than guessing
    AmbiguousIdentityMatch = 4,

    // Role matches, but the form's CurrentStage is not the target stage
    WrongStage = 5,

    // No such application/form exists
    FormNotFound = 6,

    // The form reached final approval; no further edits are possible
    FormCompleted = 7
}