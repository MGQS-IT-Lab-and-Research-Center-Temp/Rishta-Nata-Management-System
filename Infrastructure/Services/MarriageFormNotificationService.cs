// File: Infrastructure/Services/MarriageFormNotificationService.cs
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <summary>
/// Infrastructure adapter for the marriage-form notification workflow.
/// </summary>
public sealed class MarriageFormNotificationService : IMarriageFormNotificationService
{
    private readonly ILogger<MarriageFormNotificationService> _logger;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;
    private readonly RishtanataDbContext _context;

    public MarriageFormNotificationService(
        ILogger<MarriageFormNotificationService> logger,
        IEmailSender emailSender,
        IConfiguration configuration,
        RishtanataDbContext context)
    {
        _logger = logger;
        _emailSender = emailSender;
        _configuration = configuration;
        _context = context;
    }

    /// <inheritdoc />
    public async Task NotifyRevertedAsync(
        MarriageApplicationForm form,
        MarriageFormRejection rejection,
        CancellationToken cancellationToken = default)
    {
        // Get the user ID from the rejection record (which we set to the submitter's ID in the handler)
        var userId = rejection.CreatedBy;

        if (!userId.HasValue)
        {
            _logger.LogWarning("No user ID provided for rejection notification");
            return;
        }

        // Retrieve the user's email from the database
        var user = await _context.JamaatMembers
            .FirstOrDefaultAsync(m => m.Id == userId.Value, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found for rejection notification", userId);
            return;
        }

        // Generate the email content
        var (subject, body) = GenerateRejectionEmail(form, rejection, user);

        // Send the email
        await _emailSender.SendEmailAsync(user.Email, subject, body);

        _logger.LogInformation(
            "Rejection notification email sent to {UserEmail} for form {FormId} reverted from {RejectedAtStage} to {RevertedToStage}. RejectionId={RejectionId}",
            user.Email,
            form.Id,
            rejection.RejectedAtStage,
            rejection.RevertedToStage,
            rejection.Id);
    }

    private (string Subject, string Body) GenerateRejectionEmail(
        MarriageApplicationForm form,
        MarriageFormRejection rejection,
        JamaatMember user)
    {
        var baseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? string.Empty;
        var formUrl = string.IsNullOrEmpty(baseUrl)
            ? $"/forms/{form.Id}"
            : $"{baseUrl}/forms/{form.Id}";

        var subject = $"Your marriage application section has been returned for revision - {form.ReferenceNumber}";

        var body = $@"
            <html>
            <body>
                <h2>Marriage Application Returned for Revision</h2>
                <p>Dear {user.FirstName} {user.Surname},</p>
                <p>Your section of the marriage application (Reference: {form.ReferenceNumber}) has been returned for revision.</p>
                <p><strong>Reason for revision:</strong> {rejection.Reason}</p>
                <p>Please review and make the necessary corrections.</p>
                <p><a href='{formUrl}'>View Application</a></p>
                <p>Best regards,<br/>Rishtanata Team</p>
            </body>
            </html>";

        return (subject, body);
    }
}
