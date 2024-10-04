using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface ILocationOfEventsRepository : IRepository<LocationOfEvent>
{
    Task<LocationOfEvent> GetByTitleAsync(string title);
}