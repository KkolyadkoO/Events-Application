using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.Location;

public class DeleteLocationUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLocationUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var location = await _unitOfWork.Locations.GetById(id);

        if (location == null)
        {
            throw new NotFoundException($"Location with ID '{id}' not found.");
        }

        await _unitOfWork.Locations.Delete(id);
        await _unitOfWork.Complete();
    }
}