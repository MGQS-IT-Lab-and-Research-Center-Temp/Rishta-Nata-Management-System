using Infrastructure.DTOs.Imam;

namespace Application.Interfaces;

/// <summary>
/// Lists the applications awaiting the logged-in imam's post-ceremony sign-off
/// (forms at AwaitingImamSignoff where the member is the designated officiating
/// imam).
/// </summary>
public interface IImamSignoffService
{
    Task<IReadOnlyList<ImamPendingApplicationDto>> GetPendingAsync(
        string membershipNo,
        CancellationToken cancellationToken = default);
}