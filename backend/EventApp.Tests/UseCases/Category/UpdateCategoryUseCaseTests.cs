using AutoMapper;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Category;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Category;

public class UpdateCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateCategoryUseCase _updateCategoryUseCase;

    public UpdateCategoryUseCaseTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _updateCategoryUseCase = new UpdateCategoryUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_CategoryExists_UpdatesCategoryAndReturnsId()
    {
        var categoryId = Guid.NewGuid();
        var requestDto = new CategoryOfEventsRequestDto("Updated Title");
        var existingCategory = new CategoryOfEvent { Id = categoryId, Title = "Old Title" };
        var updatedCategory = new CategoryOfEvent { Id = categoryId, Title = "Updated Title" };

        _mockUnitOfWork.Setup(u => u.Categories.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        _mockMapper.Setup(m => m.Map<CategoryOfEvent>(requestDto))
            .Returns(updatedCategory);

        var result = await _updateCategoryUseCase.Execute(categoryId, requestDto);

        Assert.Equal(categoryId, result);
        _mockUnitOfWork.Verify(u => u.Categories.UpdateAsync(updatedCategory), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Execute_CategoryNotFound_ThrowsNotFoundException()
    {
        var categoryId = Guid.NewGuid();
        var requestDto = new CategoryOfEventsRequestDto("Updated Title");

        _mockUnitOfWork.Setup(u => u.Categories.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _updateCategoryUseCase.Execute(categoryId, requestDto));
        _mockUnitOfWork.Verify(u => u.Categories.UpdateAsync(It.IsAny<CategoryOfEvent>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Never);
    }
}