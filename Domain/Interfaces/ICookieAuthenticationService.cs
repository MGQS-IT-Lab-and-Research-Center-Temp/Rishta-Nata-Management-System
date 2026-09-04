using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;

public interface ICookieAuthenticationService
{
    Task SignInAsync(JamaatMember jamaatMember, IEnumerable<string> roles);
    Task SignOutAsync();
}
