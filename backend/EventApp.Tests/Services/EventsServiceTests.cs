using Moq;
using Xunit;
using EventApp.Application;
using EventApp.Core.Models;
using EventApp.Core.Abstractions;
using Microsoft.AspNetCore.Http;

namespace EventApp.Tests.Services;

public class EventsServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly EventsService _eventsService;

    public EventsServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _eventsService = new EventsService(_unitOfWorkMock.Object);
    }
    
    [Fact]
    public async Task GetAllEvents_ReturnsListOfEvents()
    {
        var eventsList = new List<Event>
        {
            new Event(Guid.NewGuid(), "Event1", "Description1", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 100, new List<MemberOfEvent>(), ""),
            new Event(Guid.NewGuid(), "Event2", "Description2", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 200, new List<MemberOfEvent>(), "")
        };

        _unitOfWorkMock.Setup(u => u.Events.Get()).ReturnsAsync(eventsList);

        var result = await _eventsService.GetAllEvents();

        Assert.Equal(eventsList.Count, result.Count);
        _unitOfWorkMock.Verify(u => u.Events.Get(), Times.Once);
    }
    
    [Fact]
    public async Task GetEventById_ReturnsEvent()
    {
        var eventId = Guid.NewGuid();
        var eventItem = new Event(eventId, "Event1", "Description1", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 100, new List<MemberOfEvent>(), "");

        _unitOfWorkMock.Setup(u => u.Events.GetById(eventId)).ReturnsAsync(eventItem);

        var result = await _eventsService.GetEventById(eventId);

        Assert.NotNull(result);
        Assert.Equal(eventId, result.Id);
        _unitOfWorkMock.Verify(u => u.Events.GetById(eventId), Times.Once);
    }

    [Fact]
    public async Task AddEvent_ReturnsEventId()
    {
        var eventId = Guid.NewGuid();
        var eventItem = new Event(eventId, "Event1", "Description1", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 100, new List<MemberOfEvent>(), "");

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1000);

        _unitOfWorkMock.Setup(u => u.Events.Create(eventItem, It.IsAny<IFormFile>())).ReturnsAsync(eventId);

        var result = await _eventsService.AddEvent(eventItem, fileMock.Object);

        Assert.Equal(eventId, result);
        _unitOfWorkMock.Verify(u => u.Events.Create(eventItem, fileMock.Object), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task UpdateEvent_UpdatesSuccessfully()
    {
        var eventId = Guid.NewGuid();
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1000);

        _unitOfWorkMock.Setup(u => u.Events.Update(eventId, "UpdatedEvent", It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), "UpdatedDescription", 200, fileMock.Object)).ReturnsAsync(eventId);

        var result = await _eventsService.UpdateEvent(eventId, "UpdatedEvent", Guid.NewGuid(), DateTime.Now, Guid.NewGuid(), "UpdatedDescription", 200, fileMock.Object);

        Assert.Equal(eventId, result);
        _unitOfWorkMock.Verify(u => u.Events.Update(eventId, "UpdatedEvent", It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), "UpdatedDescription", 200, fileMock.Object), Times.Once);
    }

    [Fact]
    public async Task DeleteEvent_DeletesSuccessfully()
    {
        var eventId = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Events.Delete(eventId)).ReturnsAsync(eventId);

        var result = await _eventsService.DeleteEvent(eventId);

        Assert.Equal(eventId, result);
        _unitOfWorkMock.Verify(u => u.Events.Delete(eventId), Times.Once);
    }

    [Fact]
    public async Task UpdateEvent_ThrowsInvalidOperationException_WhenRepositoryThrowsException()
    {
        var eventId = Guid.NewGuid();
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1000);

        var expectedExceptionMessage = "Repository error";

        _unitOfWorkMock.Setup(u => u.Events.Update(eventId, "UpdatedEvent", It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), "UpdatedDescription", 200, fileMock.Object))
            .ThrowsAsync(new Exception(expectedExceptionMessage));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _eventsService.UpdateEvent(eventId, "UpdatedEvent", Guid.NewGuid(), DateTime.Now, Guid.NewGuid(), "UpdatedDescription", 200, fileMock.Object));

        Assert.Equal(expectedExceptionMessage, exception.Message);
        _unitOfWorkMock.Verify(u => u.Events.Update(eventId, "UpdatedEvent", It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), "UpdatedDescription", 200, fileMock.Object), Times.Once);
    }

    [Fact]
    public async Task AddEvent_ThrowsInvalidOperationException_WhenRepositoryThrowsException()
    {
        var newEvent = new Event(
            Guid.NewGuid(),
            "Test Event",
            "Description",
            DateTime.Now,
            Guid.NewGuid(),
            Guid.NewGuid(),
            100,
            new List<MemberOfEvent>(),
            "/images/test.jpg"
        );

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1000);

        var expectedExceptionMessage = "Repository error";

        _unitOfWorkMock.Setup(u => u.Events.Create(newEvent, fileMock.Object))
            .ThrowsAsync(new Exception(expectedExceptionMessage));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _eventsService.AddEvent(newEvent, fileMock.Object));

        Assert.Equal(expectedExceptionMessage, exception.Message);
    
        _unitOfWorkMock.Verify(u => u.Events.Create(newEvent, fileMock.Object), Times.Once);
    
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Never);
    }

    [Fact]
    public async Task GetEventByFilters_ReturnsAllEvents_WhenNoFiltersAreProvided()
    {
        var events = new List<Event>
        {
            new Event(Guid.NewGuid(), "Event 1", "Description 1", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 50, new List<MemberOfEvent>(), "/images/test1.jpg"),
            new Event(Guid.NewGuid(), "Event 2", "Description 2", DateTime.Now.AddDays(1), Guid.NewGuid(), Guid.NewGuid(), 100, new List<MemberOfEvent>(), "/images/test2.jpg")
        };
    
        _unitOfWorkMock.Setup(u => u.Events.GetByFilters(null, null, null, null, null, null, null, null))
            .ReturnsAsync((events, events.Count));

        var (filteredEvents, totalCount) = await _eventsService.GetEventByFilters(null, null, null, null, null, null, null, null);

        Assert.Equal(events.Count, totalCount);
        Assert.Equal(events, filteredEvents);

        _unitOfWorkMock.Verify(u => u.Events.GetByFilters(null, null, null, null, null, null, null, null), Times.Once);
    }

    [Fact]
    public async Task GetEventByFilters_ReturnsFilteredEvents_WhenTitleIsProvided()
    {
        var title = "Event 1";
        var events = new List<Event>
        {
            new Event(Guid.NewGuid(), title, "Description", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 50, new List<MemberOfEvent>(), "/images/test.jpg")
        };
    
        _unitOfWorkMock.Setup(u => u.Events.GetByFilters(title, null, null, null, null, null, null, null))
            .ReturnsAsync((events, events.Count));

        var (filteredEvents, totalCount) = await _eventsService.GetEventByFilters(title, null, null, null, null, null, null, null);

        Assert.Single(filteredEvents);
        Assert.Equal(title, filteredEvents.First()?.Title);
        Assert.Equal(events.Count, totalCount);

        _unitOfWorkMock.Verify(u => u.Events.GetByFilters(title, null, null, null, null, null, null, null), Times.Once);
    }

    [Fact]
    public async Task GetEventByFilters_ReturnsPagedEvents_WhenPaginationIsProvided()
    {
        var events = new List<Event>
        {
            new Event(Guid.NewGuid(), "Event 1", "Description 1", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 50, new List<MemberOfEvent>(), "/images/test1.jpg"),
            new Event(Guid.NewGuid(), "Event 2", "Description 2", DateTime.Now.AddDays(1), Guid.NewGuid(), Guid.NewGuid(), 100, new List<MemberOfEvent>(), "/images/test2.jpg")
        };

        var page = 1;
        var size = 1;

        _unitOfWorkMock.Setup(u => u.Events.GetByFilters(null, null, null, null, null, null, page, size))
            .ReturnsAsync((events.Take(size).ToList(), events.Count));

        var (filteredEvents, totalCount) = await _eventsService.GetEventByFilters(null, null, null, null, null, null, page, size);

        Assert.Single(filteredEvents);
        Assert.Equal(events.Count, totalCount); // Total count should be the total number of events, not just the paginated result

        _unitOfWorkMock.Verify(u => u.Events.GetByFilters(null, null, null, null, null, null, page, size), Times.Once);
    }


}