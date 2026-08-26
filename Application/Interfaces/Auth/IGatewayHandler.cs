using Domain.Entities;
using Infrastructure.Identity.Tokens;

namespace Application.Interfaces.Identity;

public interface IGatewayHandler
{
    Task<string[]?> GetMemberRoleAsync(int chandaNo);

    Task<JamaatMember?> GetMemberByChandaNoAsync(int chandaNo);
    
    // Task<JamaatMember?> GetMemberByEmailAsync(string email);
    Task<MemberApiLoginResponse?> GenerateToken(TokenRequest request);
}
