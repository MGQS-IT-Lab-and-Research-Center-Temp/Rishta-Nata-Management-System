namespace Application.Interfaces
{
    public interface INotificationDispatcher
    {
        Task DispatchRejectionNotificationAsync(
            string recipientUserId,
            string formId,
            string reason,
            CancellationToken cancellationToken = default);
    }
}
