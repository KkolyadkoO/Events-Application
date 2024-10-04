using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Category;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Category;

public class AddCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddCategoryUseCase _addCategoryUseCase;

    public AddCategoryUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _addCategoryUseCase = new AddCategoryUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowDuplicateCategory_WhenCategoryExists()
    {
        var requestDto = new CategoryOfEventsRequestDto("Test Category");
        _unitOfWorkMock.Setup(u => u.Categories.GetByTitleAsync(It.IsAny<string>()))
            .ReturnsAsync(new CategoryOfEvent());

        await Assert.ThrowsAsync<DuplicateCategory>(() => _addCategoryUseCase.Execute(requestDto));
    }

    [Fact]
    public async Task Execute_ShouldAddCategory_WhenCategoryDoesNotExist()
    {
        var requestDto = new CategoryOfEventsRequestDto("New Category");
        var category = new CategoryOfEvent { Title = requestDto.Title };
        var generatedId = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Categories.GetByTitleAsync(It.IsAny<string>()))
            .ReturnsAsync((CategoryOfEvent)null);
        _mapperMock.Setup(m => m.Map<CategoryOfEvent>(requestDto)).Returns(category);
        _unitOfWorkMock.Setup(u => u.Categories.AddAsync(category)).ReturnsAsync(generatedId);

        var result = await _addCategoryUseCase.Execute(requestDto);

        Assert.Equal(generatedId, result);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}