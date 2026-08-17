using Domain.Entities;

namespace Presentation.Services.Auth;

public interface ICookieAuthenticationService
{
    Task SignInAsync(JamaatMember jamaatMember);
    Task SignOutAsync();
}
