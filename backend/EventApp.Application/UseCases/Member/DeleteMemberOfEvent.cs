using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Member;

public class DeleteMemberOfEvent
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberOfEvent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var memberOfEvent = await _unitOfWork.Members.GetByIdAsync(id);
        if (memberOfEvent == null)
        {
            throw new NotFoundException($"Member of Event with Id {id} not found");
        }

        await _unitOfWork.Members.DeleteAsync(id);
        await _unitOfWork.Complete();
    }
}