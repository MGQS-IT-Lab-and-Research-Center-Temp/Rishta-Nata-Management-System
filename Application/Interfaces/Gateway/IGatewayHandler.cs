using Domain.Entities;
using Infrastructure.Identity.Tokens;

namespace Application.Interfaces.Gateway;

/// <summary>
/// Adapter contract for the external Tajneed/member API: login (GenerateToken)
/// and member lookup (GetMemberByChandaNoAsync).
/// Cleanup: file moved from Interfaces/Auth/ into Interfaces/Gateway/ so the
/// folder mirrors its namespace (Application.Interfaces.Gateway). Implemented
/// by Gateway/Implementation/GatewayHandler.cs.
/// </summary>
public interface IGatewayHandler
{
    //Task<string[]?> GetMemberRoleAsync(string chandaNo);

    Task<JamaatMember?> GetMemberByChandaNoAsync(string chandaNo);
    
    Task<MemberApiLoginResponse?> GenerateToken(TokenRequest request);
}
