using Application.Interfaces;
using Application.Interfaces.Gateway;
using Application.Models;
using Infrastructure.Identity.Tokens;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IGatewayHandler _gatewayHandler;
    private readonly IJamaatMemberService _jamaatMemberService;

    public AuthService(IGatewayHandler gatewayHandler, IJamaatMemberService jamaatMemberService)
    {
        _gatewayHandler = gatewayHandler;
        _jamaatMemberService = jamaatMemberService;
    }

    public async Task<AuthResult> LoginAsync(string chandaNo, string password)
    {
        var tokenRequest = new TokenRequest(
            chandaNo,
            password);

        var tokenResponse = await _gatewayHandler.GenerateToken(tokenRequest);

        if (tokenResponse is null)
        {
            return AuthResult.Failure("Invalid Chanda number or password.");
        }

        if (!tokenResponse.Status || string.IsNullOrWhiteSpace(tokenResponse.Token))
        {
            return AuthResult.Failure(string.IsNullOrWhiteSpace(tokenResponse.Message)
                    ? "Login failed."
                    : tokenResponse.Message);
        }

        var jamaatMember = await _gatewayHandler.GetMemberByChandaNoAsync(chandaNo);

        if (jamaatMember is null)
        {
            return AuthResult.Failure("We could not find your member account.");
        }

        var localMember = await _jamaatMemberService.CreateOrUpdateAsync(jamaatMember);

        var roles = tokenResponse.Data?.roles ?? Array.Empty<string>();

        return AuthResult.Success(localMember, roles);
    }
}
