using EventApp.Core.Models;

namespace EventApp.DataAccess.Repositories;

public interface IRefreshTokenRepository
{
    Task<Guid> Create(Guid userId, string token, DateTime expires);
    Task<RefreshToken> Get(string refreshToken);
    Task<RefreshToken> GetByUserId(Guid userId);
    Task<string> Update(Guid tokenId, Guid userId, string token, DateTime expires);
    Task<string> Delete(string refreshToken);
}