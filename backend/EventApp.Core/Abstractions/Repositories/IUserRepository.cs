using EventApp.Core.Models;

namespace EventApp.DataAccess.Repositories;

public interface IUserRepository
{
    Task<List<User>> Get();
    Task<User> GetByEmail(string email);
    Task<User> GetByLogin(string login);
    Task<Guid> Create(User user);
    Task<User> GetById(Guid id);
}