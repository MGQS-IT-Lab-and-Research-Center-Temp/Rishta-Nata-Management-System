using Domain.Entities;
using Infrastructure.Identity.Tokens;

namespace Application.Interfaces.Identity;

public interface IGatewayHandler
{
    //Task<string[]?> GetMemberRoleAsync(string chandaNo);

    Task<JamaatMember?> GetMemberByChandaNoAsync(string chandaNo);
    
    Task<MemberApiLoginResponse?> GenerateToken(TokenRequest request);
}
