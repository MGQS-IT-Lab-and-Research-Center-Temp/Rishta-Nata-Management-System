using Domain.Entities;

namespace Domain.Interfaces;


public interface IMarriageFormNotificationService
{
    Task NotifyRevertedAsync(MarriageApplicationForm form,MarriageFormRejection rejection,CancellationToken cancellationToken = default);
}