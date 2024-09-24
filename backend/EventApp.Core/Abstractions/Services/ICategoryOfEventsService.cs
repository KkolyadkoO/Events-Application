using EventApp.Core.Models;

namespace EventApp.Application;

public interface ICategoryOfEventsService
{
    Task<List<CategoryOfEvent>> GetAllCategoryOfEvents();
    Task<CategoryOfEvent> GetCategoryOfEventById(Guid id);
    Task<CategoryOfEvent> GetCategoryOfEventByTitle(string title);
    Task<Guid> AddCategoryOfEvent(CategoryOfEvent categoryOfEvent);
    Task<Guid> UpdateCategoryOfEvent(Guid id, string title);
    Task<Guid> DeleteCategoryOfEvent(Guid id);
}