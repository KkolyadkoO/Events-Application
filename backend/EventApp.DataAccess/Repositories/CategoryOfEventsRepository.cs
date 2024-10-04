using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class CategoryOfEventsRepository : Repository<CategoryOfEvent>, ICategoryOfEventsRepository
{
    public CategoryOfEventsRepository(EventAppDBContext dbContext) : base(dbContext)
    {
    }

    public async Task<CategoryOfEvent> GetByTitleAsync(string title)
    {
        return await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title);
    }
}