using System;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IParticipantInvitationService
    {
        Task<ParticipantInvitationDto> CreateInvitationAsync(Guid applicationId, Side side, ParticipantRole role, int? witnessOrder = null);
        Task<ParticipantInvitationDto> GetInvitationByTokenAsync(string token);
        Task RevokeInvitationAsync(Guid invitationId);
        Task MarkCompletedAsync(Guid invitationId);
    }
}
