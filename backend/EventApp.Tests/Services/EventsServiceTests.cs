// using EventApp.Application;
// using EventApp.Core.Abstractions;
// using EventApp.Core.Models;
// using EventApp.DataAccess.Repositories;
// using Moq;
// using Xunit;
//
// namespace EventApp.Tests.Services;
//
// public class EventsServiceTests
// {
//     private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//     private readonly Mock<IEventsRepository> _eventRepositoryMock;
//     private readonly EventsService _eventsService;
//
//     public EventsServiceTests()
//     {
//         _unitOfWorkMock = new Mock<IUnitOfWork>();
//         _eventRepositoryMock = new Mock<IEventsRepository>();
//
//         _unitOfWorkMock.Setup(u => u.Events).Returns(_eventRepositoryMock.Object);
//         _eventsService = new EventsService(_unitOfWorkMock.Object);
//     }
//
//     [Fact]
//     public async Task GetAllEvents_ReturnsListOfEvents()
//     {
//         var events = new List<Event>
//         {
//             new Event(Guid.NewGuid(), "Event 1", "Description 1", DateTime.UtcNow, "Location 1", Guid.NewGuid(), 100,
//                 new List<MemberOfEvent>(), "http://example.com/event1.jpg"),
//             new Event(Guid.NewGuid(), "Event 2", "Description 2", DateTime.UtcNow, "Location 2", Guid.NewGuid(), 200,
//                 new List<MemberOfEvent>(), "http://example.com/event2.jpg")
//         };
//
//         _eventRepositoryMock
//             .Setup(e => e.Get())
//             .ReturnsAsync(events);
//
//         var result = await _eventsService.GetAllEvents();
//
//         Assert.NotNull(result);
//         Assert.Equal(2, result.Count);
//         Assert.Equal("Event 1", result[0].Title);
//         Assert.Equal("Event 2", result[1].Title);
//     }
//
//     [Fact]
//     public async Task GetEventById_ReturnsEvent()
//     {
//         var eventId = Guid.NewGuid();
//         var eventItem = new Event(eventId, "Event 1", "Description 1", DateTime.UtcNow, "Location 1", Guid.NewGuid(),
//             100, new List<MemberOfEvent>(), "http://example.com/event1.jpg");
//
//         _eventRepositoryMock
//             .Setup(e => e.GetById(eventId))
//             .ReturnsAsync(eventItem);
//
//         var result = await _eventsService.GetEventById(eventId);
//
//         Assert.NotNull(result);
//         Assert.Equal(eventId, result.Id);
//         Assert.Equal("Event 1", result.Title);
//     }
//
//     [Fact]
//     public async Task GetEventByTitle_ReturnsEvent()
//     {
//         var eventItem = new Event(Guid.NewGuid(), "Event 1", "Description 1", DateTime.UtcNow, "Location 1",
//             Guid.NewGuid(), 100, new List<MemberOfEvent>(), "http://example.com/event1.jpg");
//
//         _eventRepositoryMock
//             .Setup(e => e.GetByTitle("Event 1"))
//             .ReturnsAsync(eventItem);
//
//         var result = await _eventsService.GetEventByTitle("Event 1");
//
//         Assert.NotNull(result);
//         Assert.Equal("Event 1", result.Title);
//     }
//
//     // [Fact]
//     // public async Task GetEventByFilters_ReturnsFilteredEvents()
//     // {
//     //     var events = new List<Event?>
//     //     {
//     //         new Event(Guid.NewGuid(), "Filtered Event", "Filtered Description", DateTime.UtcNow, "Location 1",
//     //             Guid.NewGuid(), 100, new List<MemberOfEvent>(), "http://example.com/filtered.jpg")
//     //     };
//     //
//     //     _eventRepositoryMock
//     //         .Setup(e => e.GetByFilters(It.IsAny<string>(), It.IsAny<string>(),
//     //             It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<Guid?>(),
//     //             It.IsAny<Guid?>(), It.IsAny<int?>(), It.IsAny<int?>()))
//     //         .ReturnsAsync(events);
//     //
//     //     var result = await _eventsService.GetEventByFilters("Filtered Event", null, null, null, null, null, null, null);
//     //
//     //     Assert.NotNull(result);
//     //     Assert.Single(result);
//     //     Assert.Equal("Filtered Event", result[0]?.Title);
//     // }
//
//     [Fact]
//     public async Task GetEventByPage_ReturnsPagedEvents()
//     {
//         var pagedEvents = new List<Event>
//         {
//             new Event(Guid.NewGuid(), "Event 1", "Description 1", DateTime.UtcNow, "Location 1", Guid.NewGuid(), 100,
//                 new List<MemberOfEvent>(), "http://example.com/event1.jpg"),
//             new Event(Guid.NewGuid(), "Event 2", "Description 2", DateTime.UtcNow, "Location 2", Guid.NewGuid(), 200,
//                 new List<MemberOfEvent>(), "http://example.com/event2.jpg")
//         };
//
//         _eventRepositoryMock
//             .Setup(e => e.GetByPage(1, 10))
//             .ReturnsAsync(pagedEvents);
//
//         var result = await _eventsService.GetEventByPage(1, 10);
//
//         Assert.NotNull(result);
//         Assert.Equal(2, result.Count);
//         Assert.Equal("Event 1", result[0].Title);
//         Assert.Equal("Event 2", result[1].Title);
//     }
//
//     [Fact]
//     public async Task AddEvent_AddsEventAndReturnsId()
//     {
//         var newEvent = new Event(Guid.NewGuid(), "New Event", "New Description", DateTime.UtcNow, "New Location",
//             Guid.NewGuid(), 100, new List<MemberOfEvent>(), "http://example.com/new.jpg");
//
//         _eventRepositoryMock
//             .Setup(e => e.Create(newEvent))
//             .ReturnsAsync(newEvent.Id);
//
//         var result = await _eventsService.AddEvent(newEvent);
//
//         Assert.Equal(newEvent.Id, result);
//         _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
//     }
//
//     [Fact]
//     public async Task AddEvent_ThrowsInvalidOperationException_OnFailure()
//     {
//         var newEvent = new Event(Guid.NewGuid(), "New Event", "New Description", DateTime.UtcNow, "New Location",
//             Guid.NewGuid(), 100, new List<MemberOfEvent>(), "http://example.com/new.jpg");
//
//         _eventRepositoryMock
//             .Setup(e => e.Create(newEvent))
//             .ThrowsAsync(new Exception("Test exception"));
//
//         var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
//             _eventsService.AddEvent(newEvent));
//
//         Assert.Equal("Test exception", exception.Message);
//     }
//
//     [Fact]
//     public async Task UpdateEvent_UpdatesEventAndReturnsId()
//     {
//         var eventId = Guid.NewGuid();
//         var updatedEvent = new Event(eventId, "Updated Event", "Updated Description", DateTime.UtcNow,
//             "Updated Location", Guid.NewGuid(), 100, new List<MemberOfEvent>(), "http://example.com/updated.jpg");
//
//         _eventRepositoryMock
//             .Setup(e => e.Update(eventId, updatedEvent.Title, updatedEvent.Location, updatedEvent.Date,
//                 updatedEvent.CategoryId, updatedEvent.Description, updatedEvent.MaxNumberOfMembers,
//                 updatedEvent.ImageUrl))
//             .ReturnsAsync(eventId);
//
//         var result = await _eventsService.UpdateEvent(eventId, updatedEvent.Title, updatedEvent.Location,
//             updatedEvent.Date, updatedEvent.CategoryId, updatedEvent.Description, updatedEvent.MaxNumberOfMembers,
//             updatedEvent.ImageUrl);
//
//         Assert.Equal(eventId, result);
//     }
//
//     [Fact]
//     public async Task DeleteEvent_DeletesEventAndReturnsId()
//     {
//         var eventId = Guid.NewGuid();
//
//         _eventRepositoryMock
//             .Setup(e => e.Delete(eventId))
//             .ReturnsAsync(eventId);
//
//         var result = await _eventsService.DeleteEvent(eventId);
//
//         Assert.Equal(eventId, result);
//     }
//
//     [Fact]
//     public async Task UpdateEvent_ThrowsInvalidOperationException_OnFailure()
//     {
//         var eventId = Guid.NewGuid();
//         var title = "Event Title";
//         var location = "Event Location";
//         var date = DateTime.Now;
//         var categoryId = Guid.NewGuid();
//         var description = "Event Description";
//         var maxNumberOfMembers = 100;
//         var imageUrl = "http://example.com/image.jpg";
//
//         _unitOfWorkMock.Setup(u =>
//                 u.Events.Update(eventId, title, location, date, categoryId, description, maxNumberOfMembers, imageUrl))
//             .ThrowsAsync(new Exception("Test exception"));
//
//         var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
//             _eventsService.UpdateEvent(eventId, title, location, date, categoryId, description, maxNumberOfMembers,
//                 imageUrl));
//
//         Assert.Equal("Test exception", exception.Message);
//     }
// }