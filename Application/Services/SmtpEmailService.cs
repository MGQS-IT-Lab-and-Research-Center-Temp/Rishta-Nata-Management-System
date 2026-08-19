using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendParticipantInvitationAsync(
            string recipientEmail,
            string recipientName,
            string invitationUrl,
            string side,
            string role,
            int? witnessOrder = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                throw new ArgumentException("Recipient email is required.", nameof(recipientEmail));
            }

            if (string.IsNullOrWhiteSpace(invitationUrl))
            {
                throw new ArgumentException("Invitation URL is required.", nameof(invitationUrl));
            }

            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var fromName = _configuration["EmailSettings:FromName"] ?? "Rishta Nata Management System";
            var host = _configuration["EmailSettings:Host"];
            var portText = _configuration["EmailSettings:Port"];
            var enableSslText = _configuration["EmailSettings:EnableSsl"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(portText))
            {
                throw new InvalidOperationException("SMTP email configuration is incomplete. Configure EmailSettings in appsettings.json.");
            }

            var roleLabel = !string.IsNullOrWhiteSpace(role) ? role : "participant";
            var witnessLabel = witnessOrder.HasValue ? $" (Witness {witnessOrder})" : string.Empty;

            using var message = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = "Complete your participant details",
                IsBodyHtml = true,
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #222;'>
                        <p>Hello {recipientName},</p>
                        <p>You have been invited to complete your information for the <strong>{side}</strong> side as <strong>{roleLabel}{witnessLabel}</strong>.</p>
                        <p>Please click the link below to continue:</p>
                        <p><a href='{invitationUrl}'>Complete participant information</a></p>
                        <p>This link is unique to you and expires after a short period of time.</p>
                        <p>Regards,<br/>Rishta Nata Management System</p>
                    </body>
                    </html>"
            };

            message.To.Add(recipientEmail);

            using var smtpClient = new SmtpClient
            {
                Host = host,
                Port = int.TryParse(portText, out var port) ? port : 587,
                EnableSsl = bool.TryParse(enableSslText, out var enableSsl) && enableSsl,
                Credentials = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)
                    ? new NetworkCredential(username, password)
                    : CredentialCache.DefaultNetworkCredentials,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            await smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
