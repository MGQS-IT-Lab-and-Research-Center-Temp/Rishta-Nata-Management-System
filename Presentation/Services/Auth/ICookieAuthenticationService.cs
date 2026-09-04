using System.Threading.Tasks;
using Domain.Entities;

namespace Presentation.Services.Auth;

public interface ICookieAuthenticationService
{
    Task SignInAsync(JamaatMember jamaatMember, IEnumerable<string> roles);
    Task SignOutAsync();

}
