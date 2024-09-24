using EventApp.Core.Models;

namespace EventApp.DataAccess.Repositories;

public interface ICategoryOfEventsRepository
{
    Task<List<CategoryOfEvent>> Get();
    Task<CategoryOfEvent> GetById(Guid id);
    Task<CategoryOfEvent> GetByTitle(string title);
    Task<Guid> Add(Guid id, string title);
    Task<Guid> Update(Guid id, string title);
    Task<Guid> Delete(Guid id);
}