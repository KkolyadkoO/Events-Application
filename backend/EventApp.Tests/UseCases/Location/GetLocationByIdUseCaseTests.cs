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

public class GetLocationByIdUseCaseTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetLocationByIdUseCase _getLocationByIdUseCase;

    public GetLocationByIdUseCaseTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _getLocationByIdUseCase = new GetLocationByIdUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetLocationById_ShouldReturnLocation_WhenLocationExists()
    {
        var locationId = Guid.NewGuid();
        var locationEntity = new LocationOfEvent(locationId, "Test Location");
        var locationResponse = new LocationOfEventsResponseDto(locationId, "Test Location");

        _mockUnitOfWork.Setup(u => u.Locations.GetByIdAsync(locationId)).ReturnsAsync(locationEntity);
        _mockMapper.Setup(m => m.Map<LocationOfEventsResponseDto>(locationEntity)).Returns(locationResponse);


        var result = await _getLocationByIdUseCase.Execute(locationId);

        Assert.Equal(locationResponse.Id, result.Id);
        Assert.Equal(locationResponse.Title, result.Title);
    }

    [Fact]
    public async Task GetLocationById_ShouldThrowNotFoundException_WhenLocationDoesNotExist()
    {
        var locationId = Guid.NewGuid();

        _mockUnitOfWork.Setup(u => u.Locations.GetByIdAsync(locationId)).ReturnsAsync((LocationOfEvent)null);


        await Assert.ThrowsAsync<NotFoundException>(() => _getLocationByIdUseCase.Execute(locationId));
    }
}