using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.Category;

public class DeleteCategoryUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var category = await _unitOfWork.Categories.GetById(id);

        if (category == null)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }

        await _unitOfWork.Categories.Delete(id);
        await _unitOfWork.Complete();
    }
}