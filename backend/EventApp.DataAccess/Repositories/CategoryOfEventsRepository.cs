using AutoMapper;
using EventApp.Core.Models;
using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class CategoryOfEventsRepository : ICategoryOfEventsRepository
{
    private readonly EventAppDBContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryOfEventsRepository(EventAppDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryOfEvent>> Get()
    {
        var eventCategoriesEntities = await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .OrderBy(e => e.Title)
            .ToListAsync();
        return _mapper.Map<List<CategoryOfEvent>>(eventCategoriesEntities);
    }

    public async Task<CategoryOfEvent> GetById(Guid id)
    {
        var foundCategoryOfEvent = await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        return _mapper.Map<CategoryOfEvent>(foundCategoryOfEvent);
    }

    public async Task<CategoryOfEvent> GetByTitle(string title)
    {
        var findedEvent = await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title);
        return _mapper.Map<CategoryOfEvent>(findedEvent);
    }

    public async Task<Guid> Add(Guid id, string title)
    {
        var foundedCategory = await _dbContext.CategoryOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title);
        if (foundedCategory != null)
        {
            throw new InvalidOperationException("Category with the same title already exists. ");
        }

        var categoryOfEvent = new CategoryOfEventEntity
        {
            Id = id,
            Title = title,
        };

        await _dbContext.CategoryOfEventEntities.AddAsync(categoryOfEvent);

        return categoryOfEvent.Id;
    }

    public async Task<Guid> Update(Guid id, string title)
    {
        await _dbContext.CategoryOfEventEntities
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(c => c.Title, title));
        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _dbContext.CategoryOfEventEntities
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();
        return id;
    }
}