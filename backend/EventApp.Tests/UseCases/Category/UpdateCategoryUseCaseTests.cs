using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.UseCases.Category;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Category;

public class UpdateCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateCategoryUseCase _updateCategoryUseCase;

    public UpdateCategoryUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _updateCategoryUseCase = new UpdateCategoryUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        var categoryId = Guid.NewGuid();
        var requestDto = new CategoryOfEventsRequestDto("New Title");

        _unitOfWorkMock.Setup(u => u.Categories.GetById(categoryId))
            .ReturnsAsync((CategoryOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _updateCategoryUseCase.Execute(categoryId, requestDto));
    }

    [Fact]
    public async Task Execute_ShouldUpdateCategory_WhenCategoryExists()
    {
        var categoryId = Guid.NewGuid();
        var category = new CategoryOfEvent { Id = categoryId, Title = "Old Title" };
        var requestDto = new CategoryOfEventsRequestDto("New Title");

        _unitOfWorkMock.Setup(u => u.Categories.GetById(categoryId))
            .ReturnsAsync(category);

        var result = await _updateCategoryUseCase.Execute(categoryId, requestDto);

        Assert.Equal(categoryId, result);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}