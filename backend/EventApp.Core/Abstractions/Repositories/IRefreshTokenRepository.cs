using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string refreshToken);
    Task<RefreshToken> GetByUserIdAsync(Guid userId);
    Task DeleteByTokenAsync(string refreshToken);
}