using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class EmailNotificationDispatcher : INotificationDispatcher
    {
        private readonly IEmailService _emailService;
        
        private readonly IUserService _userService;

        public EmailNotificationDispatcher(IEmailService emailService, IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
        }

        public async Task DispatchRejectionNotificationAsync(
            string recipientUserId,
            string formId,
            string reason,
            CancellationToken cancellationToken = default)
        {
            // 1. Retrieve the user's email address
            var user = await _userService.GetUserByIdAsync(recipientUserId);
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                // Log warning: Cannot notify user with ID {recipientUserId} - email not found.
                return;
            }

            // 2. Construct the email content
            var subject = $"Action Required: Marriage Application Section Rejected (Form: {formId})";
            var body = $@"
                <p>Dear {user.FirstName},</p>
                <p>A section you submitted for Marriage Application <strong>{formId}</strong> has been rejected.</p>
                <p><strong>Reason:</strong> {reason}</p>
                <p>Please log in to the portal to review and correct the information.</p>
            ";

            // 3. Send the email using the existing service
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
    }
}
