using EventApp.Core.Models;
using Microsoft.AspNetCore.Http;

namespace EventApp.Application;

public interface IEventsService
{
    Task<List<Event>> GetAllEvents();
    Task<Event?> GetEventById(Guid id);
    Task<Event?> GetEventByTitle(string title);

    Task<(List<Event?>, int)> GetEventByFilters(string? title, Guid? locationId, DateTime? startDate,
        DateTime? endDate, Guid? category, Guid? userId, int? page, int? size);

    Task<List<Event>> GetEventByPage(int page, int size);
    Task<Guid> AddEvent(Event receivedEvent, IFormFile imageFile);

    Task<Guid> UpdateEvent(Guid id, string title, Guid locationId, DateTime date, Guid category,
        string description, int maxNumberOfMembers, IFormFile? imageFile);

    Task<Guid> DeleteEvent(Guid id);
}