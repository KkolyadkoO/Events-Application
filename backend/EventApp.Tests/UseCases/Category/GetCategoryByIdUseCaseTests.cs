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

public class GetCategoryByIdUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCategoryByIdUseCase _getCategoryByIdUseCase;

    public GetCategoryByIdUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _getCategoryByIdUseCase = new GetCategoryByIdUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        var categoryId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _getCategoryByIdUseCase.Execute(categoryId));
    }

    [Fact]
    public async Task Execute_ShouldReturnCategory_WhenCategoryExists()
    {
        var categoryId = Guid.NewGuid();
        var category = new CategoryOfEvent { Id = categoryId, Title = "Category 1" };
        var responseDto = new CategoryOfEventsResponseDto(categoryId, "Category 1");

        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId))
            .ReturnsAsync(category);
        _mapperMock.Setup(m => m.Map<CategoryOfEventsResponseDto>(category))
            .Returns(responseDto);

        var result = await _getCategoryByIdUseCase.Execute(categoryId);

        Assert.Equal(responseDto, result);
    }
}