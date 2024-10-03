using EventApp.Core.Models;
using EventApp.DataAccess;
using EventApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventApp.Tests.Repositories;

public class UserRepositoryTests
{
    private DbContextOptions<EventAppDBContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<EventAppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Create_ShouldAddUserAndReturnId()
    {
        var options = CreateInMemoryOptions();
        var user = new User { Id = Guid.NewGuid(), UserName = "testUser", UserEmail = "test@mail.com" };

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var resultId = await repository.Create(user);
            await context.SaveChangesAsync();

            var addedUser = await context.UserEntities.FindAsync(resultId);
            Assert.NotNull(addedUser);
            Assert.Equal(user.UserName, addedUser.UserName);
            Assert.Equal(user.UserEmail, addedUser.UserEmail);
        }
    }

    [Fact]
    public async Task Get_ShouldReturnAllUsers()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            context.UserEntities.AddRange(
                new User { Id = Guid.NewGuid(), UserName = "User1", UserEmail = "user1@mail.com" },
                new User { Id = Guid.NewGuid(), UserName = "User2", UserEmail = "user2@mail.com" }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var result = await repository.Get();

            Assert.Equal(2, result.Count);
            Assert.Equal("User1", result[0].UserName);
            Assert.Equal("User2", result[1].UserName);
        }
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnUserIfExists()
    {
        var options = CreateInMemoryOptions();
        var userEmail = "test@mail.com";
        var userId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            await context.UserEntities.AddAsync(new User { Id = userId, UserName = "testUser", UserEmail = userEmail });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var result = await repository.GetByEmail(userEmail);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(userEmail, result.UserEmail);
        }
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var result = await repository.GetByEmail("non-existent@mail.com");

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnUserIfExists()
    {
        var options = CreateInMemoryOptions();
        var userId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            await context.UserEntities.AddAsync(new User
                { Id = userId, UserName = "testUser", UserEmail = "test@mail.com" });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var result = await repository.GetById(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var result = await repository.GetById(Guid.NewGuid());

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetByLogin_ShouldReturnUserIfExists()
    {
        var options = CreateInMemoryOptions();
        var userLogin = "testUser";
        var userId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            await context.UserEntities.AddAsync(new User
                { Id = userId, UserName = userLogin, UserEmail = "test@mail.com" });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var result = await repository.GetByLogin(userLogin);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(userLogin, result.UserName);
        }
    }

    [Fact]
    public async Task GetByLogin_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new UserRepository(context);

            var result = await repository.GetByLogin("non-existent");

            Assert.Null(result);
        }
    }
}