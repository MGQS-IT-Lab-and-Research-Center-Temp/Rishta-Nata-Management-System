using Application.Interfaces;
using Application.Interfaces.Gateway;
using Application.Models;
using Infrastructure.Identity.Tokens;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IGatewayHandler _gatewayHandler;
    private readonly IJamaatMemberService _jamaatMemberService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IGatewayHandler gatewayHandler,
        IJamaatMemberService jamaatMemberService,
        ILogger<AuthService> logger)
    {
        _gatewayHandler = gatewayHandler;
        _jamaatMemberService = jamaatMemberService;
        _logger = logger;
    }

    public async Task<AuthResult> LoginAsync(string chandaNo, string password)
    {
        try
        {
            var tokenRequest = new TokenRequest(chandaNo, password);

            var (tokenResponse, errorMessage) = await _gatewayHandler.GenerateToken(tokenRequest);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return AuthResult.Failure(errorMessage);
            }

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

            var jamaatMember = await _gatewayHandler.GetMemberByMemberNoAsync(chandaNo);

            if (jamaatMember is null)
            {
                return AuthResult.Failure("We could not find your member account.");
            }

            var roles = tokenResponse.Data?.roles ?? Array.Empty<string>();

            jamaatMember.Roles = string.Join(",", roles);

            var localMember = await _jamaatMemberService.CreateOrUpdateAsync(jamaatMember);

            return AuthResult.Success(localMember, roles);
        }
        catch (Exception ex) when (
            ex is HttpRequestException or TaskCanceledException or TimeoutException)
        {
            // Transport-level failure (DNS, connection refused, timeout,
            // gateway 5xx). Never present this as a bad-credentials message —
            // surface a clear "service unreachable" hint instead.
            var sanitizedChandaNo = (chandaNo ?? string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);

            _logger.LogError(ex,
                "Tajneed service unreachable during login for ChandaNo {ChandaNo}.", sanitizedChandaNo);

            return AuthResult.Failure(
                "The member service is temporarily unreachable. Please try again in a few moments.");
        }
    }
}
