using AutoMapper;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class LocationOfEventsRepository : ILocationOfEventsRepository
{
    private readonly EventAppDBContext _dbContext;

    public LocationOfEventsRepository(EventAppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LocationOfEvent>> Get()
    {
        var locationsOfEvent = await _dbContext.LocationsOfEventEntities
            .AsNoTracking()
            .OrderBy(e => e.Title)
            .ToListAsync();
        return locationsOfEvent;
    }

    public async Task<LocationOfEvent> GetById(Guid id)
    {
        var foundLocationOfEvent = await _dbContext.LocationsOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        return foundLocationOfEvent;
    }
    
    public async Task<LocationOfEvent> GetByTitle(string title)
    {
        var foundedLocationOfEvent = await _dbContext.LocationsOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title);
        return foundedLocationOfEvent;
    }

    public async Task<Guid> Add(LocationOfEvent locationOfEvent)
    {
        await _dbContext.LocationsOfEventEntities.AddAsync(locationOfEvent);
        return locationOfEvent.Id;
    }

    public async Task<bool> Update(LocationOfEvent locationOfEvent)
    {
        var foundedLocation = await _dbContext.LocationsOfEventEntities
            .FirstOrDefaultAsync(e => e.Id == locationOfEvent.Id);

        if (foundedLocation == null)
        {
            return false;
        }

        foundedLocation.Title = locationOfEvent.Title;
        
        _dbContext.LocationsOfEventEntities.Update(foundedLocation);
        
        return true;
    }

    public async Task Delete(Guid id)
    {
        var entity = await _dbContext.LocationsOfEventEntities
            .FirstOrDefaultAsync(e => e.Id == id);
        if (entity != null)
        {
            _dbContext.LocationsOfEventEntities.Remove(entity);
        }
    }
}