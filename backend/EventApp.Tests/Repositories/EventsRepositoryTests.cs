using EventApp.Core.Models;
using EventApp.DataAccess;
using EventApp.DataAccess.Repositories;
using EventApp.Core.Specifications;
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
                var result = await repository.Get();

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
                var result = await repository.GetById(eventId);

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
                var result = await repository.GetById(Guid.NewGuid());

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

                var resultId = await repository.Create(eventToAdd);
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

                await repository.Update(eventToUpdate);
                await context.SaveChangesAsync();

                var updatedEvent = await context.EventEntities.FindAsync(eventId);
                Assert.NotNull(updatedEvent);
                Assert.Equal("Updated Title", updatedEvent.Title);
            }
        }

        [Fact]
        public async Task Update_ShouldNotModifyIfEventDoesNotExist()
        {
            var options = CreateInMemoryOptions();
            var eventToUpdate = new Event { Id = Guid.NewGuid(), Title = "Non-existent Event", Date = DateTime.Now };

            using (var context = new EventAppDBContext(options))
            {
                var repository = new EventsRepository(context);

                await repository.Update(eventToUpdate);
                await context.SaveChangesAsync();

                var updatedEvent = await context.EventEntities.FindAsync(eventToUpdate.Id);
                Assert.Null(updatedEvent);
            }
        }

        [Fact]
        public async Task Delete_ShouldRemoveEventIfExists()
        {
            var options = CreateInMemoryOptions();
            var eventId = Guid.NewGuid();

            using (var context = new EventAppDBContext(options))
            {
                await context.EventEntities.AddAsync(new Event { Id = eventId, Title = "Event To Delete", Date = DateTime.Now });
                await context.SaveChangesAsync();
            }

            using (var context = new EventAppDBContext(options))
            {
                var repository = new EventsRepository(context);

                await repository.Delete(eventId);
                await context.SaveChangesAsync();

                var deletedEvent = await context.EventEntities.FindAsync(eventId);
                Assert.Null(deletedEvent);
            }
        }

        [Fact]
        public async Task Delete_ShouldNotFailIfEventDoesNotExist()
        {
            var options = CreateInMemoryOptions();
    
            using (var context = new EventAppDBContext(options))
            {
                await context.EventEntities.AddAsync(new Event { Id = Guid.NewGuid(), Title = "Existing Event", Date = DateTime.Now });
                await context.SaveChangesAsync();
            }

            using (var context = new EventAppDBContext(options))
            {
                var repository = new EventsRepository(context);

                var initialCount = await context.EventEntities.CountAsync();

                await repository.Delete(Guid.NewGuid()); 
                await context.SaveChangesAsync();

                var finalCount = await context.EventEntities.CountAsync();
                Assert.Equal(initialCount, finalCount);
            }
        }

    }

