using AutoMapper;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.UseCases.Location;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.UseCases.Location;

public class GetAllLocationsUseCaseTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllLocationsUseCase _getAllLocationsUseCase;
    
    public GetAllLocationsUseCaseTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _getAllLocationsUseCase = new GetAllLocationsUseCase(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllLocations_ShouldReturnListOfLocations()
    {
        var locationEntities = new List<LocationOfEvent>
        {
            new LocationOfEvent(Guid.NewGuid(), "Location 1"),
            new LocationOfEvent(Guid.NewGuid(), "Location 2")
        };
        var locationResponseDtos = new List<LocationOfEventsResponseDto>
        {
            new LocationOfEventsResponseDto(Guid.NewGuid(), "Location 1"),
            new LocationOfEventsResponseDto(Guid.NewGuid(), "Location 2")
        };

        _mockUnitOfWork.Setup(u => u.Locations.Get()).ReturnsAsync(locationEntities);
        _mockMapper.Setup(m => m.Map<List<LocationOfEventsResponseDto>>(locationEntities)).Returns(locationResponseDtos);


        var result = await _getAllLocationsUseCase.Execute();

        Assert.Equal(2, result.Count);
    }
}