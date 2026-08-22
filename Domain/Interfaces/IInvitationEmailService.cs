using Domain.Entities;

namespace Domain.Interfaces;

public interface IInvitationEmailService
{
    Task SendAsync(Invitation invitation, string emailTo);
}