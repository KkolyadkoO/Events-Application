using AutoMapper;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class CategoryOfEventsRepository : ICategoryOfEventsRepository
{
    private readonly EventAppDBContext _dbContext;

    public CategoryOfEventsRepository(EventAppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CategoryOfEvent>> Get()
    {
        var eventCategories = await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .OrderBy(e => e.Title)
            .ToListAsync();
        return eventCategories;
    }

    public async Task<CategoryOfEvent> GetById(Guid id)
    {
        var foundCategoryOfEvent = await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        return foundCategoryOfEvent;
    }

    public async Task<CategoryOfEvent> GetByTitle(string title)
    {
        var foundedCategoryOfEvent = await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title);
        return foundedCategoryOfEvent;
    }

    public async Task<Guid> Add(CategoryOfEvent categoryOfEvent)
    {
        await _dbContext.CategoryOfEventEntities.AddAsync(categoryOfEvent);
        return categoryOfEvent.Id;
    }

    public async Task<bool> Update(CategoryOfEvent categoryOfEvent)
    {
        var foundedCategory = await _dbContext.CategoryOfEventEntities
            .FirstOrDefaultAsync(e => e.Id == categoryOfEvent.Id);

        if (foundedCategory == null)
        {
            return false;
        }

        foundedCategory.Title = categoryOfEvent.Title;

        _dbContext.CategoryOfEventEntities.Update(foundedCategory);

        return true;
    }

    public async Task Delete(Guid id)
    {
        var entity = await _dbContext.CategoryOfEventEntities
            .FirstOrDefaultAsync(e => e.Id == id);
        if (entity != null)
        {
            _dbContext.CategoryOfEventEntities.Remove(entity);
        }
    }
}