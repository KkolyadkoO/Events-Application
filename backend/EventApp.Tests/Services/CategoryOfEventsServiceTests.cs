using EventApp.Application;
using EventApp.Core.Abstractions;
using EventApp.Core.Models;
using EventApp.DataAccess.Repositories;
using Moq;
using Xunit;

namespace EventApp.Tests.Services;

public class CategoryOfEventsServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryOfEventsRepository> _categoryRepositoryMock;
    private readonly CategoryOfEventsService _categoryOfEventsService;

    public CategoryOfEventsServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryOfEventsRepository>();
        
        _unitOfWorkMock.Setup(u => u.Categories).Returns(_categoryRepositoryMock.Object);
        _categoryOfEventsService = new CategoryOfEventsService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllCategoryOfEvents_ReturnsListOfCategories()
    {
        var categories = new List<CategoryOfEvent>
        {
            new CategoryOfEvent( Guid.NewGuid(), "Music" ),
            new CategoryOfEvent( Guid.NewGuid(), "Technology" )
        };

        _categoryRepositoryMock
            .Setup(c => c.Get())
            .ReturnsAsync(categories);

        var result = await _categoryOfEventsService.GetAllCategoryOfEvents();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Music", result[0].Title);
        Assert.Equal("Technology", result[1].Title);
    }

    [Fact]
    public async Task GetCategoryOfEventById_ReturnsCategory()
    {
        var category = new CategoryOfEvent (Guid.NewGuid(), "Sports" );

        _categoryRepositoryMock
            .Setup(c => c.GetById(category.Id))
            .ReturnsAsync(category);

        var result = await _categoryOfEventsService.GetCategoryOfEventById(category.Id);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal("Sports", result.Title);
    }

    [Fact]
    public async Task GetCategoryOfEventByTitle_ReturnsCategory()
    {
        var category = new CategoryOfEvent (Guid.NewGuid(), "Arts" );


        _categoryRepositoryMock
            .Setup(c => c.GetByTitle("Arts"))
            .ReturnsAsync(category);

        var result = await _categoryOfEventsService.GetCategoryOfEventByTitle("Arts");

        Assert.NotNull(result);
        Assert.Equal("Arts", result.Title);
    }

    [Fact]
    public async Task AddCategoryOfEvent_AddsCategoryAndReturnsId()
    {
        var category = new CategoryOfEvent (Guid.NewGuid(), "Science" );


        _categoryRepositoryMock
            .Setup(c => c.Add(category.Id, category.Title))
            .ReturnsAsync(category.Id);

        var result = await _categoryOfEventsService.AddCategoryOfEvent(category);

        Assert.Equal(category.Id, result);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once); 
    }

    [Fact]
    public async Task AddCategoryOfEvent_ThrowsInvalidOperationException_OnFailure()
    {
        var category = new CategoryOfEvent (Guid.NewGuid(), "Sports" );


        _categoryRepositoryMock
            .Setup(c => c.Add(It.IsAny<Guid>(), It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _categoryOfEventsService.AddCategoryOfEvent(category));

        Assert.Equal("Failed to add the category: Test exception", exception.Message);
    }

    [Fact]
    public async Task UpdateCategoryOfEvent_UpdatesCategoryAndReturnsId()
    {
        var categoryId = Guid.NewGuid();
        var updatedTitle = "Updated Title";

        _categoryRepositoryMock
            .Setup(c => c.Update(categoryId, updatedTitle))
            .ReturnsAsync(categoryId);

        var result = await _categoryOfEventsService.UpdateCategoryOfEvent(categoryId, updatedTitle);

        Assert.Equal(categoryId, result);
    }

    [Fact]
    public async Task DeleteCategoryOfEvent_DeletesCategoryAndReturnsId()
    {
        var categoryId = Guid.NewGuid();

        _categoryRepositoryMock
            .Setup(c => c.Delete(categoryId))
            .ReturnsAsync(categoryId);

        var result = await _categoryOfEventsService.DeleteCategoryOfEvent(categoryId);

        Assert.Equal(categoryId, result);
    }
}