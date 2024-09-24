using AutoMapper;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using EventApp.DataAccess.Entities;
using EventApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly EventAppDBContext _dbContex;

    public UserRepository(EventAppDBContext dbContext, IMapper mapper)
    {
        _dbContex = dbContext;
        _mapper = mapper;
    }

    public async Task<List<User>> Get()
    {
        var users = _dbContex.UserEntities
            .AsNoTracking()
            .ToListAsync();
        return _mapper.Map<List<User>>(users.Result);
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _dbContex.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserEmail == email);
        return _mapper.Map<User>(user);
    }
    public async Task<User> GetById(Guid id)
    {
        var user = await _dbContex.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id) ?? throw new UserNotFound("User does not exist");
        return _mapper.Map<User>(user);
    }

    public async Task<User> GetByLogin(string login)
    {
        var user = await _dbContex.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == login);
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        return _mapper.Map<User>(user);
    }

    public async Task<Guid> Create(User user)
    {
        var createdUser = new UserEntity
        {
            Id = Guid.NewGuid(),
            MemberOfEvents = [],
            Password = user.Password,
            UserName = user.UserName,
            UserEmail = user.UserEmail,
            Role = user.Role
        };

        await _dbContex.UserEntities.AddAsync(createdUser);
        return createdUser.Id;
    }
}