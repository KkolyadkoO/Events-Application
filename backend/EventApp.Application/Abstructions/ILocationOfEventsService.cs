using EventApp.Core.Models;

namespace EventApp.Application;

public interface ILocationOfEventsService
{
    Task<List<LocationOfEvent>> GetAllLocationOfEvents();
    Task<LocationOfEvent> GetLocationOfEventById(Guid id);
    Task<Guid> AddLocationOfEvent(LocationOfEvent locationOfEvent);
    Task<Guid> UpdateLocationOfEvent(Guid id, string title);
    Task<Guid> DeleteLocationOfEvent(Guid id);
}