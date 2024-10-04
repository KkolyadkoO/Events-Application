using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Event;

public class DeleteEventUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
        if (existingEvent == null)
        {
            throw new NotFoundException("Event not found");
        }

        await _unitOfWork.Events.DeleteAsync(id);
        await _unitOfWork.Complete();
    }
}