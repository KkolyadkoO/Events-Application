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

            var result = await repository.GetByIdAsync(locationId);

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

            var result = await repository.GetByIdAsync(Guid.NewGuid());

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

            var resultId = await repository.AddAsync(location);
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

            await repository.UpdateAsync(location);
            await context.SaveChangesAsync();

            var updatedLocation = await context.LocationsOfEventEntities.FindAsync(location.Id);
            Assert.Equal("Updated Title", updatedLocation.Title);
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

            await repository.DeleteAsync(location.Id);
            await context.SaveChangesAsync();

            var deletedLocation = await context.LocationsOfEventEntities.FindAsync(location.Id);
            Assert.Null(deletedLocation);
        }
    }
    
    

    [Fact]
    public async Task GetByTitle_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new LocationOfEventsRepository(context);

            var result = await repository.GetByTitleAsync("Non-existing Location");

            Assert.Null(result);
        }
    }
}