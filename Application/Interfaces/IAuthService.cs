using Application.Models;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string chandaNo, string password);
}
