namespace EventApp.Infrastructure;

public interface IJwtTokenService
{
    Task<(string accessToken, string refreshToken)> GenerateToken(Guid userId, string userName, string role);
    string GenerateRefreshToken();
}