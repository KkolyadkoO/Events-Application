using EventApp.Core.Abstractions;
using EventApp.Core.Models;
using EventApp.DataAccess.Repositories;

namespace EventApp.Application;

public class CategoryOfEventsService : ICategoryOfEventsService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryOfEventsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CategoryOfEvent>> GetAllCategoryOfEvents()
    {
        return await _unitOfWork.Categories.Get();
    }

    public async Task<CategoryOfEvent> GetCategoryOfEventById(Guid id)
    {
        return await _unitOfWork.Categories.GetById(id);
    }

    public async Task<CategoryOfEvent> GetCategoryOfEventByTitle(string title)
    {
        return await _unitOfWork.Categories.GetByTitle(title);
    }

    public async Task<Guid> AddCategoryOfEvent(CategoryOfEvent categoryOfEvent)
    {
        try
        {
            var id = await _unitOfWork.Categories.Add(categoryOfEvent.Id, categoryOfEvent.Title);
            _unitOfWork.Complete();
            return id;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException("Failed to add the category: " + ex.Message);
        }
    }

    public async Task<Guid> UpdateCategoryOfEvent(Guid id, string title)
    {
        return await _unitOfWork.Categories.Update(id, title);
    }

    public async Task<Guid> DeleteCategoryOfEvent(Guid id)
    {
        return await _unitOfWork.Categories.Delete(id);
    }
}