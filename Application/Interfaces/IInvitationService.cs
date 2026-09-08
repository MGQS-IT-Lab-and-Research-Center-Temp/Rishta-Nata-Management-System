using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

/// <summary>
/// Invitation token lifecycle: generate, validate, mark-used.
/// </summary>
public interface IInvitationService
{
    // Generates a new invitation token, persists it, and optionally sends email.
    Task<Invitation> GenerateInvitationAsync(InvitationTargetType targetType, Guid? marriageApplicationId, string? marriageReferenceNumber, Guid? recipientJamaatMemberId, string? recipientMembershipNo, string? createdBy, string? emailTo = null);

    //  Validates a token and returns the Invitation if valid (not expired and not used).
    Task<Invitation?> ValidateTokenAsync(string token);

    // Marks an invitation as used.
    Task MarkUsedAsync(Guid invitationId);
}
