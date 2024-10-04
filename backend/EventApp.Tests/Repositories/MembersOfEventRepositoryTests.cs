using EventApp.Core.Models;
using EventApp.DataAccess;
using EventApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventApp.Tests.Repositories;

public class MembersOfEventRepositoryTests
{
    private DbContextOptions<EventAppDBContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<EventAppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Get_ShouldReturnAllMembers()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            context.MemberOfEventEntities.AddRange(
                new MemberOfEvent { Id = Guid.NewGuid(), Name = "John", LastName = "Doe" },
                new MemberOfEvent { Id = Guid.NewGuid(), Name = "Jane", LastName = "Doe" }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var result = await repository.GetAllAsync();

            Assert.Equal(2, result.Count);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnMemberIfExists()
    {
        var options = CreateInMemoryOptions();
        var memberId = Guid.NewGuid();
        using (var context = new EventAppDBContext(options))
        {
            context.MemberOfEventEntities.Add(new MemberOfEvent { Id = memberId, Name = "John", LastName = "Doe" });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var result = await repository.GetByIdAsync(memberId);

            Assert.NotNull(result);
            Assert.Equal(memberId, result.Id);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetByEventId_ShouldReturnMembersForEvent()
    {
        var options = CreateInMemoryOptions();
        var eventId = Guid.NewGuid();
        using (var context = new EventAppDBContext(options))
        {
            context.MemberOfEventEntities.AddRange(
                new MemberOfEvent { Id = Guid.NewGuid(), EventId = eventId, Name = "John" },
                new MemberOfEvent { Id = Guid.NewGuid(), EventId = eventId, Name = "Jane" }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var result = await repository.GetByEventIdAsync(eventId);

            Assert.Equal(2, result.Count);
            Assert.All(result, m => Assert.Equal(eventId, m.EventId));
        }
    }

    [Fact]
    public async Task GetByUserId_ShouldReturnMembersForUser()
    {
        var options = CreateInMemoryOptions();
        var userId = Guid.NewGuid();
        using (var context = new EventAppDBContext(options))
        {
            context.MemberOfEventEntities.AddRange(
                new MemberOfEvent { Id = Guid.NewGuid(), UserId = userId, Name = "John" },
                new MemberOfEvent { Id = Guid.NewGuid(), UserId = userId, Name = "Jane" }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var result = await repository.GetByUserIdAsync(userId);

            Assert.Equal(2, result.Count);
            Assert.All(result, m => Assert.Equal(userId, m.UserId));
        }
    }

    [Fact]
    public async Task GetByEventIdAndUserId_ShouldReturnMemberIfExists()
    {
        var options = CreateInMemoryOptions();
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        using (var context = new EventAppDBContext(options))
        {
            var member = new MemberOfEvent { Id = Guid.NewGuid(), EventId = eventId, UserId = userId, Name = "John" };
            context.MemberOfEventEntities.Add(member);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var result = await repository.GetByEventIdAndUserIdAsync(eventId, userId);

            Assert.NotNull(result);
            Assert.Equal(eventId, result.EventId);
            Assert.Equal(userId, result.UserId);
        }
    }

    [Fact]
    public async Task GetByEventIdAndUserId_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var result = await repository.GetByEventIdAndUserIdAsync(Guid.NewGuid(), Guid.NewGuid());

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task Create_ShouldAddMemberAndReturnId()
    {
        var options = CreateInMemoryOptions();
        var member = new MemberOfEvent { Id = Guid.NewGuid(), Name = "New Member", LastName = "Doe" };

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            var resultId = await repository.AddAsync(member);
            await context.SaveChangesAsync();

            var addedMember = await context.MemberOfEventEntities.FindAsync(resultId);
            Assert.NotNull(addedMember);
            Assert.Equal(member.Name, addedMember.Name);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateMemberAndReturnTrue()
    {
        var options = CreateInMemoryOptions();
        var member = new MemberOfEvent { Id = Guid.NewGuid(), Name = "John", LastName = "Doe" };

        using (var context = new EventAppDBContext(options))
        {
            context.MemberOfEventEntities.Add(member);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);
            member.Name = "Updated Name";

            await repository.UpdateAsync(member);
            await context.SaveChangesAsync();

            var updatedMember = await context.MemberOfEventEntities.FindAsync(member.Id);
            Assert.Equal("Updated Name", updatedMember.Name);
        }
    }

    [Fact]
    public async Task Delete_ShouldRemoveMemberIfExists()
    {
        var options = CreateInMemoryOptions();
        var member = new MemberOfEvent { Id = Guid.NewGuid(), Name = "To be deleted" };

        using (var context = new EventAppDBContext(options))
        {
            context.MemberOfEventEntities.Add(member);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            await repository.DeleteAsync(member.Id);
            await context.SaveChangesAsync();

            var deletedMember = await context.MemberOfEventEntities.FindAsync(member.Id);
            Assert.Null(deletedMember);
        }
    }

    [Fact]
    public async Task DeleteByEventIdAndUserId_ShouldRemoveMemberIfExists()
    {
        var options = CreateInMemoryOptions();
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var member = new MemberOfEvent { Id = Guid.NewGuid(), EventId = eventId, UserId = userId, Name = "John" };

        using (var context = new EventAppDBContext(options))
        {
            context.MemberOfEventEntities.Add(member);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new MembersOfEventRepository(context);

            await repository.DeleteByEventIdAndUserIdAsync(eventId, userId);
            await context.SaveChangesAsync();

            var deletedMember = await context.MemberOfEventEntities
                .FirstOrDefaultAsync(m => m.EventId == eventId && m.UserId == userId);
            Assert.Null(deletedMember);
        }
    }
    
}