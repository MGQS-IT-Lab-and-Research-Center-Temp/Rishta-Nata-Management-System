using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class InvitationEmailService : IInvitationEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public InvitationEmailService(IEmailSender emailSender, IConfiguration configuration)
    {
        _emailSender = emailSender;
        _configuration = configuration;
    }

    public Task SendAsync(Invitation invitation, string emailTo)
    {
        var baseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? string.Empty;
        var link = string.IsNullOrEmpty(baseUrl)
            ? $"/invitations/{invitation.Token}"
            : $"{baseUrl}/invitations/{invitation.Token}";

        var subject = "You have been invited to complete marriage application form";

        var body = $@"<html>
            <body>
                <p>Dear Sir/Madam,</p>
                <p>You have been invited to complete the marriage application form. Please click the link below to proceed:</p>
                <p><a href='{link}'>Complete Marriage Application Form</a></p>
                <p>This invitation will expire in 7 days.</p>
                <p>Best regards,<br/>Rishtanata Team</p>
            </body>
        </html>";

        return _emailSender.SendEmailAsync(
            emailTo,
            subject,
            body);
    }
}