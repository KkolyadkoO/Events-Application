using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(EventAppDBContext dbContext) : base(dbContext) { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbContext.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserEmail == email);
    }

    public async Task<User> GetByLoginAsync(string login)
    {
        return await _dbContext.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == login);
    }
}