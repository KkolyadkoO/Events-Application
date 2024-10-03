using EventApp.Core.Models;
using EventApp.DataAccess;
using EventApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventApp.Tests.Repositories;

public class LocationOfEventsRepositoryTests
{
    private DbContextOptions<EventAppDBContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<EventAppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Get_ShouldReturnLocationsOrderedByTitle()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            context.LocationsOfEventEntities.AddRange(
                new LocationOfEvent { Id = Guid.NewGuid(), Title = "B" },
                new LocationOfEvent { Id = Guid.NewGuid(), Title = "A" }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var result = await repository.Get();

            Assert.Equal(2, result.Count);
            Assert.Equal("A", result[0].Title);
            Assert.Equal("B", result[1].Title);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnLocationIfExists()
    {
        var options = CreateInMemoryOptions();
        var locationId = Guid.NewGuid();
        using (var context = new EventAppDBContext(options))
        {
            context.LocationsOfEventEntities.Add(new LocationOfEvent { Id = locationId, Title = "Test Location" });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var result = await repository.GetById(locationId);

            Assert.NotNull(result);
            Assert.Equal(locationId, result.Id);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var result = await repository.GetById(Guid.NewGuid());

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task Add_ShouldAddLocationAndReturnId()
    {
        var options = CreateInMemoryOptions();
        var location = new LocationOfEvent { Id = Guid.NewGuid(), Title = "New Location" };

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var resultId = await repository.Add(location);
            await context.SaveChangesAsync();

            var addedLocation = await context.LocationsOfEventEntities.FindAsync(resultId);
            Assert.NotNull(addedLocation);
            Assert.Equal(location.Title, addedLocation.Title);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateLocationAndReturnTrue()
    {
        var options = CreateInMemoryOptions();
        var location = new LocationOfEvent { Id = Guid.NewGuid(), Title = "Old Title" };

        using (var context = new EventAppDBContext(options))
        {
            context.LocationsOfEventEntities.Add(location);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);
            location.Title = "Updated Title";

            var result = await repository.Update(location);
            await context.SaveChangesAsync();

            Assert.True(result);
            var updatedLocation = await context.LocationsOfEventEntities.FindAsync(location.Id);
            Assert.Equal("Updated Title", updatedLocation.Title);
        }
    }

    [Fact]
    public async Task Update_ShouldReturnFalseIfLocationDoesNotExist()
    {
        var options = CreateInMemoryOptions();
        var location = new LocationOfEvent { Id = Guid.NewGuid(), Title = "Non-existing Location" };

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var result = await repository.Update(location);

            Assert.False(result);
        }
    }

    [Fact]
    public async Task Delete_ShouldRemoveLocationIfExists()
    {
        var options = CreateInMemoryOptions();
        var location = new LocationOfEvent { Id = Guid.NewGuid(), Title = "To be deleted" };

        using (var context = new EventAppDBContext(options))
        {
            context.LocationsOfEventEntities.Add(location);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            await repository.Delete(location.Id);
            await context.SaveChangesAsync();

            var deletedLocation = await context.LocationsOfEventEntities.FindAsync(location.Id);
            Assert.Null(deletedLocation);
        }
    }

    [Fact]
    public async Task Delete_ShouldDoNothingIfLocationDoesNotExist()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var initialCount = await context.LocationsOfEventEntities.CountAsync();
            await repository.Delete(Guid.NewGuid());
            await context.SaveChangesAsync();

            var finalCount = await context.LocationsOfEventEntities.CountAsync();
            Assert.Equal(initialCount, finalCount);
        }
    }

    [Fact]
    public async Task GetByTitle_ShouldReturnLocationIfExists()
    {
        var options = CreateInMemoryOptions();
        var locationTitle = "Test Location";
        var locationId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            context.LocationsOfEventEntities.Add(new LocationOfEvent { Id = locationId, Title = locationTitle });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var result = await repository.GetByTitle(locationTitle);

            Assert.NotNull(result);
            Assert.Equal(locationId, result.Id);
            Assert.Equal(locationTitle, result.Title);
        }
    }

    [Fact]
    public async Task GetByTitle_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var result = await repository.GetByTitle("Non-existing Location");

            Assert.Null(result);
        }
    }
}