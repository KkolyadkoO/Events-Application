using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Location;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Location;

public class UpdateLocationUseCaseTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateLocationUseCase _updateLocationUseCase;

    public UpdateLocationUseCaseTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _updateLocationUseCase = new UpdateLocationUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_LocationExists_UpdatesLocationAndReturnsId()
    {
        var locationId = Guid.NewGuid();
        var requestDto = new LocationOfEventsRequestDto("Updated Title");
        var existingLocation = new LocationOfEvent { Id = locationId, Title = "Old Title" };
        var updatedLocation = new LocationOfEvent { Id = locationId, Title = "Updated Title" };

        _mockUnitOfWork.Setup(u => u.Locations.GetByIdAsync(locationId))
            .ReturnsAsync(existingLocation);

        _mockMapper.Setup(m => m.Map<LocationOfEvent>(requestDto))
            .Returns(updatedLocation);

        var result = await _updateLocationUseCase.Execute(locationId, requestDto);

        Assert.Equal(locationId, result);
        _mockUnitOfWork.Verify(u => u.Locations.UpdateAsync(updatedLocation), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Execute_LocationNotFound_ThrowsNotFoundException()
    {
        var locationId = Guid.NewGuid();
        var requestDto = new LocationOfEventsRequestDto("Updated Title");

        _mockUnitOfWork.Setup(u => u.Locations.GetByIdAsync(locationId))
            .ReturnsAsync((LocationOfEvent)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _updateLocationUseCase.Execute(locationId, requestDto));
        _mockUnitOfWork.Verify(u => u.Locations.UpdateAsync(It.IsAny<LocationOfEvent>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Never);
    }
}