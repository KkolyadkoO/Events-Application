using EventApp.Core.Models;
using EventApp.DataAccess;
using EventApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventApp.Tests.Repositories;

public class CategoryOfEventsRepositoryTests
{
    private DbContextOptions<EventAppDBContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<EventAppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Get_ShouldReturnCategoriesOrderedByTitle()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            context.CategoryOfEventEntities.AddRange(
                new CategoryOfEvent { Id = Guid.NewGuid(), Title = "B" },
                new CategoryOfEvent { Id = Guid.NewGuid(), Title = "A" }
            );
            context.SaveChanges();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);

            var result = await repository.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("B", result[0].Title);
            Assert.Equal("A", result[1].Title);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnCategoryIfExists()
    {
        var options = CreateInMemoryOptions();
        var categoryId = Guid.NewGuid();
        using (var context = new EventAppDBContext(options))
        {
            context.CategoryOfEventEntities.Add(new CategoryOfEvent { Id = categoryId, Title = "Test" });
            context.SaveChanges();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);

            var result = await repository.GetByIdAsync(categoryId);

            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();
        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task Add_ShouldAddCategoryAndReturnId()
    {
        var options = CreateInMemoryOptions();
        var category = new CategoryOfEvent { Id = Guid.NewGuid(), Title = "New Category" };

        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);

            var resultId = await repository.AddAsync(category);

            var addedCategory = await context.CategoryOfEventEntities.FindAsync(resultId);
            Assert.NotNull(addedCategory);
            Assert.Equal(category.Title, addedCategory.Title);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateCategoryAndReturnTrue()
    {
        var options = CreateInMemoryOptions();
        var category = new CategoryOfEvent { Id = Guid.NewGuid(), Title = "Old Title" };

        using (var context = new EventAppDBContext(options))
        {
            context.CategoryOfEventEntities.Add(category);
            context.SaveChanges();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);
            category.Title = "Updated Title";

            await repository.UpdateAsync(category);
            
            var updatedCategory = await context.CategoryOfEventEntities.FindAsync(category.Id);
            Assert.Equal("Updated Title", updatedCategory.Title);
        }
    }
    

    [Fact]
    public async Task Delete_ShouldRemoveCategoryIfExists()
    {
        var options = CreateInMemoryOptions();
        var category = new CategoryOfEvent { Id = Guid.NewGuid(), Title = "To be deleted" };

        using (var context = new EventAppDBContext(options))
        {
            context.CategoryOfEventEntities.Add(category);
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);

            await repository.DeleteAsync(category.Id);
            await context.SaveChangesAsync();

            var deletedCategory = await context.CategoryOfEventEntities.FindAsync(category.Id);
            Assert.Null(deletedCategory);
        }
    }

    


    [Fact]
    public async Task GetByTitle_ShouldReturnCategoryIfExists()
    {
        var options = CreateInMemoryOptions();
        var categoryTitle = "Test Category";
        var categoryId = Guid.NewGuid();

        using (var context = new EventAppDBContext(options))
        {
            context.CategoryOfEventEntities.Add(new CategoryOfEvent { Id = categoryId, Title = categoryTitle });
            await context.SaveChangesAsync();
        }

        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);

            var result = await repository.GetByTitleAsync(categoryTitle);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal(categoryTitle, result.Title);
        }
    }

    [Fact]
    public async Task GetByTitle_ShouldReturnNullIfNotExists()
    {
        var options = CreateInMemoryOptions();

        using (var context = new EventAppDBContext(options))
        {
            var repository = new CategoryOfEventsRepository(context);

            var result = await repository.GetByTitleAsync("Non-existing Category");

            Assert.Null(result);
        }
    }
}