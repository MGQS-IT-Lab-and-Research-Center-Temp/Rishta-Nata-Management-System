
using Application.Interfaces.Service;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Application.Services;

public class NotificationService : INotificationService
{
    private readonly RishtanataDbContext _dbContext;

    public NotificationService(RishtanataDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task NotifyTurnAsync(
        Guid userId,
        string title,
        string message,
        string actionUrl,
        CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            ActionUrl = actionUrl,
            IsRead = false
        };

        await _dbContext.Notifications.AddAsync(
            notification,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}