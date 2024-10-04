using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class LocationOfEventsRepository : Repository<LocationOfEvent>, ILocationOfEventsRepository
{
    public LocationOfEventsRepository(EventAppDBContext dbContext) : base(dbContext) { }

    public async Task<LocationOfEvent> GetByTitleAsync(string title)
    {
        return await _dbContext.Set<LocationOfEvent>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title);
    }
}