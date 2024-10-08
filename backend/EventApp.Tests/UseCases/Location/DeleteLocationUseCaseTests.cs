using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Location;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Location;

public class DeleteLocationUseCaseTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeleteLocationUseCase _deleteLocationUseCase;

    public DeleteLocationUseCaseTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _deleteLocationUseCase = new DeleteLocationUseCase(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task DeleteLocation_ShouldDeleteLocation_WhenLocationExists()
    {
        var locationId = Guid.NewGuid();
        var locationEntity = new LocationOfEvent(locationId, "Test Location");

        _mockUnitOfWork.Setup(u => u.Locations.GetByIdAsync(locationId)).ReturnsAsync(locationEntity);


        await _deleteLocationUseCase.Execute(locationId);

        _mockUnitOfWork.Verify(u => u.Locations.DeleteAsync(locationId), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task DeleteLocation_ShouldThrowNotFoundException_WhenLocationDoesNotExist()
    {
        var locationId = Guid.NewGuid();

        _mockUnitOfWork.Setup(u => u.Locations.GetByIdAsync(locationId)).ReturnsAsync((LocationOfEvent)null);


        await Assert.ThrowsAsync<NotFoundException>(() => _deleteLocationUseCase.Execute(locationId));
    }
}