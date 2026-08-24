using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <summary>
/// Infrastructure adapter for the marriage-form notification workflow.
/// </summary>
public sealed class MarriageFormNotificationService : IMarriageFormNotificationService
{
    private readonly ILogger<MarriageFormNotificationService> _logger;

    public MarriageFormNotificationService(ILogger<MarriageFormNotificationService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task NotifyRevertedAsync(
        MarriageApplicationForm form,
        MarriageFormRejection rejection,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Marriage form {FormId} reverted from {RejectedAtStage} to {RevertedToStage}; notification queued for Epic E. RejectionId={RejectionId}",
            form.Id,
            rejection.RejectedAtStage,
            rejection.RevertedToStage,
            rejection.Id);

        return Task.CompletedTask;
    }
}