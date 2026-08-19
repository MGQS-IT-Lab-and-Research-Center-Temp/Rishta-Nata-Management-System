namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendParticipantInvitationAsync(
            string recipientEmail,
            string recipientName,
            string invitationUrl,
            string side,
            string role,
            int? witnessOrder = null,
            CancellationToken cancellationToken = default);
    }
}
