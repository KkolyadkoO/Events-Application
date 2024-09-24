using EventApp.Core.Models;

namespace EventApp.DataAccess.Repositories;

public interface ILocationOfEventsRepository
{
    Task<List<LocationOfEvent>> Get();
    Task<LocationOfEvent> GetById(Guid id);
    Task<Guid> Add(Guid id, string title);
    Task<Guid> Update(Guid id, string title);
    Task<Guid> Delete(Guid id);
}