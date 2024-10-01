using EventApp.Application;
using EventApp.Core.Abstractions;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Moq;
using Xunit;

namespace EventApp.Tests.Services;

public class LocationOfEventsServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILocationOfEventsRepository> _locationRepositoryMock;
    private readonly LocationOfEventsService _locationOfEventsService;

    public LocationOfEventsServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _locationRepositoryMock = new Mock<ILocationOfEventsRepository>();
        
        _unitOfWorkMock.Setup(u => u.Locations).Returns(_locationRepositoryMock.Object);
        _locationOfEventsService = new LocationOfEventsService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllLocationsOfEvents_ReturnsListOfCategories()
    {
        var locations = new List<LocationOfEvent>
        {
            new LocationOfEvent( Guid.NewGuid(), "Minsk" ),
            new LocationOfEvent( Guid.NewGuid(), "Moscow" )
        };

        _locationRepositoryMock
            .Setup(c => c.Get())
            .ReturnsAsync(locations);

        var result = await _locationOfEventsService.GetAllLocationOfEvents();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Minsk", result[0].Title);
        Assert.Equal("Moscow", result[1].Title);
    }

    [Fact]
    public async Task GetLocationOfEventById_ReturnsCategory()
    {
        var location = new LocationOfEvent(Guid.NewGuid(), "Minsk" );

        _locationRepositoryMock
            .Setup(c => c.GetById(location.Id))
            .ReturnsAsync(location);

        var result = await _locationOfEventsService.GetLocationOfEventById(location.Id);

        Assert.NotNull(result);
        Assert.Equal(location.Id, result.Id);
        Assert.Equal("Minsk", result.Title);
    }
    

    [Fact]
    public async Task AddCLocationOfEvent_AddsCategoryAndReturnsId()
    {
        var location = new LocationOfEvent(Guid.NewGuid(), "Minsk" );


        _locationRepositoryMock
            .Setup(c => c.Add(location.Id, location.Title))
            .ReturnsAsync(location.Id);

        var result = await _locationOfEventsService.AddLocationOfEvent(location);

        Assert.Equal(location.Id, result);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once); 
    }

    [Fact]
    public async Task AddLocationOfEvent_ThrowsInvalidOperationException_OnFailure()
    {
        var location = new LocationOfEvent(Guid.NewGuid(), "Minsk" );


        _locationRepositoryMock
            .Setup(c => c.Add(It.IsAny<Guid>(), It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _locationOfEventsService.AddLocationOfEvent(location));

        Assert.Equal("Failed to add the location: Test exception", exception.Message);
    }

    [Fact]
    public async Task UpdateLocationOfEvent_UpdatesCategoryAndReturnsId()
    {
        var locationId = Guid.NewGuid();
        var updatedTitle = "Updated Title";

        _locationRepositoryMock
            .Setup(c => c.Update(locationId, updatedTitle))
            .ReturnsAsync(locationId);

        var result = await _locationOfEventsService.UpdateLocationOfEvent(locationId, updatedTitle);

        Assert.Equal(locationId, result);
    }

    [Fact]
    public async Task DeleteLocationOfEvent_DeletesCategoryAndReturnsId()
    {
        var locationId = Guid.NewGuid();

        _locationRepositoryMock
            .Setup(c => c.Delete(locationId))
            .ReturnsAsync(locationId);

        var result = await _locationOfEventsService.DeleteLocationOfEvent(locationId);

        Assert.Equal(locationId, result);
    }
}