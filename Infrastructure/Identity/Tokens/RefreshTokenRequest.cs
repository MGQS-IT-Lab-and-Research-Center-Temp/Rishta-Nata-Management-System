namespace Infrastructure.Identity.Tokens;

public record RefreshTokenRequest(string Token, string RefreshToken);
