using Moq;
using Xunit;
using EventApp.Application.UseCases.Category;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;

public class DeleteCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteCategoryUseCase _deleteCategoryUseCase;

    public DeleteCategoryUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _deleteCategoryUseCase = new DeleteCategoryUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        var categoryId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Categories.GetById(categoryId))
            .ReturnsAsync((CategoryOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _deleteCategoryUseCase.Execute(categoryId));
    }

    [Fact]
    public async Task Execute_ShouldDeleteCategory_WhenCategoryExists()
    {
        var categoryId = Guid.NewGuid();
        var category = new CategoryOfEvent { Id = categoryId };

        _unitOfWorkMock.Setup(u => u.Categories.GetById(categoryId))
            .ReturnsAsync(category);
        _unitOfWorkMock.Setup(u => u.Categories.Delete(categoryId))
            .Returns(Task.CompletedTask);

        await _deleteCategoryUseCase.Execute(categoryId);

        _unitOfWorkMock.Verify(u => u.Categories.Delete(categoryId), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }
}