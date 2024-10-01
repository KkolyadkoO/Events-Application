using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<Guid> Create(RefreshToken refreshToken);
    Task<RefreshToken> Get(string refreshToken);
    Task<RefreshToken> GetByUserId(Guid userId);
    Task Update(RefreshToken refreshToken);
    Task Delete(string refreshToken);
}