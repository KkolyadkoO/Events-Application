using EventApp.Application.Specifications;
using EventApp.Core.Models;
using EventApp.DataAccess;
using EventApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventApp.Tests.Repositories;

public class EventsRepositoryTests
{
    private DbContextOptions<EventAppDBContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<EventAppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Get_ShouldReturnAllEvents()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            context.EventEntities.AddRange(
                new Event { Id = Guid.NewGuid(), Title = "Event1", Date = DateTime.Now },
                new Event { Id = Guid.NewGuid(), Title = "Event2", Date = DateTime.Now.AddDays(1) }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new EventsRepository(context);
            var result = await repository.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("Event1", result[0].Title);
            Assert.Equal("Event2", result[1].Title);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnEventIfExists()
    {
        var options = CreateInMemoryOptions();
        var eventId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            await context.EventEntities.AddAsync(new Event { Id = eventId, Title = "Test Event", Date = DateTime.Now });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new EventsRepository(context);
            var result = await repository.GetByIdAsync(eventId);

            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal("Test Event", result.Title);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new EventsRepository(context);
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetBySpecificationAsync_ShouldReturnFilteredEvents()
    {
        var options = CreateInMemoryOptions();
        var today = DateTime.Now;
        var categoryId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            context.EventEntities.AddRange(
                new Event { Id = Guid.NewGuid(), Title = "Event1", Date = today, CategoryId = categoryId },
                new Event { Id = Guid.NewGuid(), Title = "Event2", Date = today.AddDays(1) }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var spec = new EventSpecification(null, null, today, null, categoryId, null);
            var repository = new EventsRepository(context);

            var (events, count) = await repository.GetBySpecificationAsync(spec, null, null);

            Assert.Single(events);
            Assert.Equal("Event1", events[0].Title);
            Assert.Equal(1, count);
        }
    }

    [Fact]
    public async Task Create_ShouldAddEventAndReturnId()
    {
        var options = CreateInMemoryOptions();
        var eventToAdd = new Event { Id = Guid.NewGuid(), Title = "New Event", Date = DateTime.Now };

        using (var context = new EventAppDBContext(options))
        {
            var repository = new EventsRepository(context);

            var resultId = await repository.AddAsync(eventToAdd);
            await context.SaveChangesAsync();

            var addedEvent = await context.EventEntities.FindAsync(resultId);
            Assert.NotNull(addedEvent);
            Assert.Equal("New Event", addedEvent.Title);
        }
    }

    [Fact]
    public async Task Update_ShouldModifyEventIfExists()
    {
        var options = CreateInMemoryOptions();
        var eventId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            await context.EventEntities.AddAsync(new Event { Id = eventId, Title = "Old Title", Date = DateTime.Now });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new EventsRepository(context);
            var eventToUpdate = new Event { Id = eventId, Title = "Updated Title", Date = DateTime.Now.AddDays(1) };

            await repository.UpdateAsync(eventToUpdate);
            await context.SaveChangesAsync();

            var updatedEvent = await context.EventEntities.FindAsync(eventId);
            Assert.NotNull(updatedEvent);
            Assert.Equal("Updated Title", updatedEvent.Title);
        }
    }
    

    [Fact]
    public async Task Delete_ShouldRemoveEventIfExists()
    {
        var options = CreateInMemoryOptions();
        var eventId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            await context.EventEntities.AddAsync(new Event
                { Id = eventId, Title = "Event To Delete", Date = DateTime.Now });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new EventsRepository(context);

            await repository.DeleteAsync(eventId);
            await context.SaveChangesAsync();

            var deletedEvent = await context.EventEntities.FindAsync(eventId);
            Assert.Null(deletedEvent);
        }
    }
}