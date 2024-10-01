using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface ICategoryOfEventsRepository
{
    Task<List<CategoryOfEvent>> Get();
    Task<CategoryOfEvent> GetById(Guid id);
    Task<CategoryOfEvent> GetByTitle(string title);
    Task<Guid> Add(CategoryOfEvent categoryOfEvent);
    Task<bool> Update(CategoryOfEvent categoryOfEvent);
    Task Delete(Guid id);
}