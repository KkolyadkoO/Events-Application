using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface IEventsRepository : IRepository<Event>
{
    Task<(List<Event>, int)> GetBySpecificationAsync(ISpecification<Event> spec, int? page, int? size);
}