using AutoMapper;
using EventApp.Core.Models;
using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class LocationOfEventsRepository : ILocationOfEventsRepository
{
    private readonly EventAppDBContext _dbContext;
    private readonly IMapper _mapper;
    
    public LocationOfEventsRepository(EventAppDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<LocationOfEvent>> Get()
    {
        var locationsOfEventEntities = await _dbContext.LocationsOfEventEntities
            .AsNoTracking()
            .OrderBy(e => e.Title)
            .ToListAsync();
        return _mapper.Map<List<LocationOfEvent>>(locationsOfEventEntities);
    }
   
    public async Task<LocationOfEvent> GetById(Guid id)
    {
        var foundLocationOfEvent =  await _dbContext.LocationsOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        return _mapper.Map<LocationOfEvent>(foundLocationOfEvent);
    }

    public async Task<Guid> Add(Guid id, string title)
    {
        var foundedLocation = _dbContext.LocationsOfEventEntities
            .AsNoTracking()
            .FirstOrDefault(e => e.Title == title);
        if (foundedLocation != null)
        {
            throw new InvalidOperationException("Location with the same title already exists. ");

        }
        var locationOfEvent = new LocationOfEventEntity
        {
            Id = id,
            Title = title,
        };
            await _dbContext.LocationsOfEventEntities.AddAsync(locationOfEvent);
        
        return locationOfEvent.Id;
    }
    public async Task<Guid> Update(Guid id, string title)
    {
        await _dbContext.LocationsOfEventEntities
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(c => c.Title, title));
        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _dbContext.LocationsOfEventEntities
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();
        return id;
    }
}