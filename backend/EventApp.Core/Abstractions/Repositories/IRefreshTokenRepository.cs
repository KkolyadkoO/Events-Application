using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<Guid> Create(RefreshToken refreshToken);
    Task<RefreshToken> Get(string refreshToken);
    Task<RefreshToken> GetByUserId(Guid userId);
    Task<bool> Update(RefreshToken refreshToken);
    Task<bool> Delete(string refreshToken);
}