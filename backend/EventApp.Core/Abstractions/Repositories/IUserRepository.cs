using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByLoginAsync(string login);
}