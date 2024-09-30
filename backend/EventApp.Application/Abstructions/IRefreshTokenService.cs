using EventApp.Core.Models;

namespace EventApp.Application;

public interface IRefreshTokenService
{
    Task<(string, string)> RefreshToken(string refreshToken);
    Task<string> DeleteRefreshToken(string refreshToken);
    Task<RefreshToken> GetRefreshToken(string refreshToken);
}