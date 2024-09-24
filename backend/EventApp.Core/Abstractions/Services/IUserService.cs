using EventApp.Core.Models;

namespace EventApp.Application;

public interface IUserService
{
    Task Register(string username, string email, string password, string role);
    Task<User> Login(string username, string password);
    Task<List<User>> GetAllUsers();
    Task<User> GetUserById(Guid id);
}