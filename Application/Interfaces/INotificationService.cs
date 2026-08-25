namespace Application.Interfaces.Service;

public interface INotificationService
{
    Task NotifyTurnAsync(
        Guid userId,
        string title,
        string message,
        string actionUrl,
        CancellationToken cancellationToken = default);
}