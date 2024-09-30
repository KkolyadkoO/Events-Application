using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EventAppDBContext _dbContex;

    public UserRepository(EventAppDBContext dbContext)
    {
        _dbContex = dbContext;
    }

    public async Task<List<User>> Get()
    {
        var users = await _dbContex.UserEntities
            .AsNoTracking()
            .ToListAsync();
        return users;
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _dbContex.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserEmail == email);
        return user;
    }
    public async Task<User> GetById(Guid id)
    {
        var user = await _dbContex.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User> GetByLogin(string login)
    {
        var user = await _dbContex.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == login);

        return user;
    }

    public async Task<Guid> Create(User user)
    {
        await _dbContex.UserEntities.AddAsync(user);
        return user.Id;
    }
}