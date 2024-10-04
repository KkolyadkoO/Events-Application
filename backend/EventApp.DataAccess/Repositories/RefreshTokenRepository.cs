using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(EventAppDBContext dbContext) : base(dbContext)
    {
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await _dbContext.Set<RefreshToken>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<RefreshToken> GetByUserIdAsync(Guid userId)
    {
        return await _dbContext.Set<RefreshToken>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task DeleteByTokenAsync(string token)
    {
        var refreshToken = await _dbContext.Set<RefreshToken>()
            .FirstOrDefaultAsync(rt => rt.Token == token);

        _dbContext.Set<RefreshToken>().Remove(refreshToken);
    }
}