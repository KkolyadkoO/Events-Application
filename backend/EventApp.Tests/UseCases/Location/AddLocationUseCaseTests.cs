using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Location;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using EventApp.DataAccess.Abstractions;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Location;

public class AddLocationUseCaseTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AddLocationUseCase _addLocationUseCase;

    public AddLocationUseCaseTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _addLocationUseCase = new AddLocationUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AddLocation_ShouldReturnNewLocationId_WhenLocationIsUnique()
    {
        var locationRequest = new LocationOfEventsRequestDto("Test Location");
        var locationEntity = new LocationOfEvent(Guid.NewGuid(), "Test Location");

        _mockUnitOfWork.Setup(u => u.Locations.GetByTitleAsync(It.IsAny<string>())).ReturnsAsync((LocationOfEvent)null);
        _mockMapper.Setup(m => m.Map<LocationOfEvent>(locationRequest)).Returns(locationEntity);
        _mockUnitOfWork.Setup(u => u.Locations.AddAsync(It.IsAny<LocationOfEvent>())).ReturnsAsync(locationEntity.Id);


        var result = await _addLocationUseCase.Execute(locationRequest);

        Assert.Equal(locationEntity.Id, result);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task AddLocation_ShouldThrowDuplicateCategory_WhenLocationAlreadyExists()
    {
        var locationRequest = new LocationOfEventsRequestDto("Existing Location");
        var existingLocation = new LocationOfEvent(Guid.NewGuid(), "Existing Location");

        _mockUnitOfWork.Setup(u => u.Locations.GetByTitleAsync(It.IsAny<string>())).ReturnsAsync(existingLocation);


        await Assert.ThrowsAsync<DuplicateCategory>(() => _addLocationUseCase.Execute(locationRequest));
    }
}