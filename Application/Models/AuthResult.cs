using Domain.Entities;

namespace Application.Models;

public class AuthResult
{
    public bool Succeeded { get; init; }
    public string? ErrorMessage { get; init; }
    public JamaatMember? Member { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; }
        = Array.Empty<string>();

    public static AuthResult Success(JamaatMember member, IEnumerable<string>? roles = null)
    {
        return new AuthResult
        {
            Succeeded = true,
            Member = member,
            Roles = roles?.ToArray() ?? Array.Empty<string>()
        };
    }

    public static AuthResult Failure(string message)
    {
        return new AuthResult
        {
            Succeeded = false,
            ErrorMessage = message
        };
    }
}