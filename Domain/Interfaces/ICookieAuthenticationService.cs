using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;

public interface ICookieAuthenticationService
{
    Task<string> SignInAsync(JamaatMember jamaatMember);
    Task SignOutAsync();
}
