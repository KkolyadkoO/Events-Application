using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.UseCases.Location;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
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
    public async Task UpdateLocation_ShouldUpdateLocation_WhenLocationExists()
    {
        var locationId = Guid.NewGuid();
        var locationEntity = new LocationOfEvent(locationId, "Old Title");
        var requestDto = new LocationOfEventsRequestDto("New Title");

        _mockUnitOfWork.Setup(u => u.Locations.GetById(locationId)).ReturnsAsync(locationEntity);


        var result = await _updateLocationUseCase.Execute(locationId, requestDto);

        Assert.Equal(locationId, result);
        Assert.Equal("New Title", locationEntity.Title);
        _mockUnitOfWork.Verify(u => u.Locations.Update(locationEntity), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task UpdateLocation_ShouldThrowNotFoundException_WhenLocationDoesNotExist()
    {
        var locationId = Guid.NewGuid();
        var requestDto = new LocationOfEventsRequestDto("New Title");

        _mockUnitOfWork.Setup(u => u.Locations.GetById(locationId)).ReturnsAsync((LocationOfEvent)null);


        await Assert.ThrowsAsync<NotFoundException>(() => _updateLocationUseCase.Execute(locationId, requestDto));
    }
}