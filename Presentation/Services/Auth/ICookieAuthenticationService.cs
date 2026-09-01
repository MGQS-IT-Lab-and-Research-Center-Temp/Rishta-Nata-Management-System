using System.Threading.Tasks;
using Domain.Entities;

namespace Presentation.Services.Auth;

public interface ICookieAuthenticationService
{
    Task<string> SignInAsync(JamaatMember jamaatMember);
    Task SignOutAsync();

}
