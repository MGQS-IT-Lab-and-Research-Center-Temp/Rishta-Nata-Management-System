using Infrastructure.DTOs.Members;

namespace Application.Interfaces;

public interface IMemberLookupService
{
    Task<MemberLookupDto?> LookupAsync(
        string chandaNo,
        CancellationToken cancellationToken = default);
}
