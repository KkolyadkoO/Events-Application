using EventApp.Core.Models;
using EventApp.DataAccess;
using EventApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventApp.Tests.Repositories;

public class RefreshTokenRepositoryTests
{
    private DbContextOptions<EventAppDBContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<EventAppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Create_ShouldAddRefreshTokenAndReturnId()
    {
        var options = CreateInMemoryOptions();
        var token = new RefreshToken { Id = Guid.NewGuid(), Token = "token123", UserId = Guid.NewGuid() };

        using (var context = new EventAppDBContext(options))
        {
            var repository = new RefreshTokenRepository(context);

            var resultId = await repository.AddAsync(token);
            await context.SaveChangesAsync();

            var addedToken = await context.RefreshTokenEntities.FindAsync(resultId);
            Assert.NotNull(addedToken);
            Assert.Equal(token.Token, addedToken.Token);
        }
    }

    [Fact]
    public async Task Get_ShouldReturnRefreshTokenIfExists()
    {
        var options = CreateInMemoryOptions();
        var token = new RefreshToken { Id = Guid.NewGuid(), Token = "token123", UserId = Guid.NewGuid() };

        using (var context = new EventAppDBContext(options))
        {
            await context.RefreshTokenEntities.AddAsync(token);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new RefreshTokenRepository(context);

            var result = await repository.GetByTokenAsync(token.Token);

            Assert.NotNull(result);
            Assert.Equal(token.Token, result.Token);
        }
    }

    [Fact]
    public async Task Get_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new RefreshTokenRepository(context);

            var result = await repository.GetByTokenAsync("non-existent-token");

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetByUserId_ShouldReturnTokenIfExists()
    {
        var options = CreateInMemoryOptions();
        var userId = Guid.NewGuid();
        var token = new RefreshToken { Id = Guid.NewGuid(), Token = "token123", UserId = userId };

        using (var context = new EventAppDBContext(options))
        {
            await context.RefreshTokenEntities.AddAsync(token);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new RefreshTokenRepository(context);

            var result = await repository.GetByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }
    }

    [Fact]
    public async Task GetByUserId_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            var repository = new RefreshTokenRepository(context);

            var result = await repository.GetByUserIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateRefreshTokenIfExists()
    {
        var options = CreateInMemoryOptions();
        var token = new RefreshToken { Id = Guid.NewGuid(), Token = "token123", UserId = Guid.NewGuid() };

        using (var context = new EventAppDBContext(options))
        {
            await context.RefreshTokenEntities.AddAsync(token);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new RefreshTokenRepository(context);
            token.Token = "updatedToken";

            await repository.UpdateAsync(token);
            await context.SaveChangesAsync();

            var updatedToken = await context.RefreshTokenEntities.FindAsync(token.Id);
            Assert.Equal("updatedToken", updatedToken.Token);
        }
    }


    [Fact]
    public async Task Delete_ShouldRemoveRefreshTokenIfExists()
    {
        var options = CreateInMemoryOptions();
        var token = new RefreshToken { Id = Guid.NewGuid(), Token = "token123", UserId = Guid.NewGuid() };

        using (var context = new EventAppDBContext(options))
        {
            await context.RefreshTokenEntities.AddAsync(token);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new RefreshTokenRepository(context);

            await repository.DeleteByTokenAsync(token.Token);
            await context.SaveChangesAsync();

            var deletedToken = await context.RefreshTokenEntities.FirstOrDefaultAsync(t => t.Token == token.Token);
            Assert.Null(deletedToken);
        }
    }

    
}