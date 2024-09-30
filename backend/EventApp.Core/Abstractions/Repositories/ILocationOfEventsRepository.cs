using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface ILocationOfEventsRepository
{
    Task<List<LocationOfEvent>> Get();
    Task<LocationOfEvent> GetById(Guid id);
    Task<Guid> Add(LocationOfEvent locationOfEvent);
    Task<bool> Update(LocationOfEvent locationOfEvent);
    Task<bool> Delete(Guid id);
}