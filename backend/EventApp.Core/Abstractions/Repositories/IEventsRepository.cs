using EventApp.Core.Models;
using EventApp.Core.Specifications;

namespace EventApp.Core.Abstractions.Repositories;

public interface IEventsRepository
{
    Task<List<Event>> Get();
    Task<Event> GetById(Guid id);

    Task<(List<Event>, int)> GetBySpecificationAsync(ISpecification<Event> spec, int? page, int? size);

    Task<Guid> Create(Event receivedEvent);

    Task Update(Event receivedEvent);

    Task Delete(Guid id);
}