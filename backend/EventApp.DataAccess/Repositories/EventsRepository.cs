using AutoMapper;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using EventApp.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class EventsRepository : IEventsRepository
{
    private readonly EventAppDBContext _dbContext;

    public EventsRepository(EventAppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Event>> Get()
    {
        var eventEntities = await _dbContext.EventEntities
            .AsNoTracking()
            .Include(e => e.Members)
            .OrderBy(e => e.Date)
            .ToListAsync();
        return eventEntities;
    }

    public async Task<Event> GetById(Guid id)
    {
        var foundedEvent = await _dbContext.EventEntities
            .AsNoTracking()
            .Include(e => e.Members)
            .FirstOrDefaultAsync(e => e.Id == id);
        return foundedEvent;
    }


    public async Task<(List<Event>, int)> GetBySpecificationAsync(ISpecification<Event> spec, int? page, int? size)
    {
        var query = _dbContext.EventEntities.AsQueryable();

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = spec.OrderBy(query);
        }

        var countOfEvents = await query.CountAsync();

        if (page.HasValue && size.HasValue)
        {
            query = query.Skip((int)((page - 1) * size)).Take((int)size);
        }

        var events = await query
            .Include(e => e.Members)
            .AsNoTracking()
            .ToListAsync();

        return (events, countOfEvents);
    }


    public async Task<Guid> Create(Event receivedEvent)
    {
        await _dbContext.EventEntities.AddAsync(receivedEvent);
        return receivedEvent.Id;
    }

    public async Task Update(Event receivedEvent)
    {
        var foundedEvent = await _dbContext.EventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == receivedEvent.Id);

        if (foundedEvent != null)
        {
            foundedEvent.Title = receivedEvent.Title;
            foundedEvent.Description = receivedEvent.Description;
            foundedEvent.Date = receivedEvent.Date;
            foundedEvent.LocationId = receivedEvent.LocationId;
            foundedEvent.CategoryId = receivedEvent.CategoryId;
            foundedEvent.MaxNumberOfMembers = receivedEvent.MaxNumberOfMembers;
            foundedEvent.ImageUrl = receivedEvent.ImageUrl;

            _dbContext.EventEntities.Update(receivedEvent);
        }
    }

    public async Task Delete(Guid id)
    {
        var foundedEvent = await _dbContext.EventEntities
            .FirstOrDefaultAsync(e => e.Id == id);

        if (foundedEvent != null)
        {
            _dbContext.EventEntities.Remove(foundedEvent);
        }
    }
}