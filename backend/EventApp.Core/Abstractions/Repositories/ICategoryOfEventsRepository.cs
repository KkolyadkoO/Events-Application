using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface ICategoryOfEventsRepository : IRepository<CategoryOfEvent>
{
    Task<CategoryOfEvent> GetByTitleAsync(string title);
}