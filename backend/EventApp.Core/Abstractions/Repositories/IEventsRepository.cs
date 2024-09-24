using EventApp.Core.Models;
using Microsoft.AspNetCore.Http;

namespace EventApp.DataAccess.Repositories;

public interface IEventsRepository
{
    Task<List<Event>> Get();
    Task<Event?> GetById(Guid id);
    Task<Event?> GetByTitle(string title);

    Task<(List<Event?>, int)> GetByFilters(string? title, Guid? locationId, DateTime? startDate,
        DateTime? endDate, Guid? categoryId, Guid? userId, int? page, int? size);

    Task<List<Event>> GetByPage(int page, int size);
    Task<Guid> Create(Event receivedEvent, IFormFile imageFile);

    Task<Guid> Update(Guid id, string title, Guid location, DateTime date, Guid category,
        string description, int maxNumberOfMembers, IFormFile? imageFile);

    Task<Guid> Delete(Guid id);
}