using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface ILocationOfEventsRepository
{
    Task<List<LocationOfEvent>> Get();
    Task<LocationOfEvent> GetById(Guid id);
    Task<LocationOfEvent> GetByTitle(string title);
    Task<Guid> Add(LocationOfEvent locationOfEvent);
    Task<bool> Update(LocationOfEvent locationOfEvent);
    Task Delete(Guid id);
}