using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly EventAppDBContext _dbContext;

    public RefreshTokenRepository(EventAppDBContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Guid> Create(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokenEntities.AddAsync(refreshToken);
        return refreshToken.Id;
    }

    public async Task<RefreshToken> Get(string refreshToken)
    {
        var token = await _dbContext.RefreshTokenEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == refreshToken);
        return token;
    }

    public async Task<RefreshToken> GetByUserId(Guid userId)
    {
        var token = await _dbContext.RefreshTokenEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);

        return token;
    }

    public async Task<bool> Update(RefreshToken refreshToken)
    {
        var foundedRefreshToken = await _dbContext.RefreshTokenEntities
            .FirstOrDefaultAsync(rt => rt.Id == refreshToken.Id);

        if (foundedRefreshToken == null)
        {
            return false;
        }

        foundedRefreshToken.UserId = refreshToken.UserId;
        foundedRefreshToken.Token = refreshToken.Token;
        foundedRefreshToken.Expires = refreshToken.Expires;

        _dbContext.RefreshTokenEntities.Update(foundedRefreshToken);

        return true;
    }

    public async Task<bool> Delete(string refreshToken)
    {
        var foundedRefreshToken = await _dbContext.RefreshTokenEntities
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (foundedRefreshToken == null)
        {
            return false;
        }
        
        _dbContext.RefreshTokenEntities.Remove(foundedRefreshToken);

        return true;
    }
}